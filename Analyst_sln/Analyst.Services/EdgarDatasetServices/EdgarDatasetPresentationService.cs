using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyst.DBAccess.Contexts;
using System.Collections.Concurrent;
using log4net;
using Analyst.Domain.Edgar;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using Analyst.Domain.Edgar.Exceptions;

namespace Analyst.Services.EdgarDatasetServices
{
    public interface IEdgarDatasetPresentationService : IEdgarDatasetBaseService<EdgarDatasetPresentation>
    {
        ConcurrentDictionary<string, int> Nums { get; set; }
        ConcurrentDictionary<string, int> Renders { get; set; }
        ConcurrentDictionary<string, int> Subs { get; set; }
        ConcurrentDictionary<string, int> Tags { get; set; }
        ConcurrentDictionary<string, int> Texts { get; set; }
    }
    public class EdgarDatasetPresentationService : EdgarDatasetBaseService<EdgarDatasetPresentation>, IEdgarDatasetPresentationService
    {
        private const string FIELD_SEPARATOR = ";";
        private const string LINE_SEPARATOR = "\n";

        public ConcurrentDictionary<string, int> Renders { get; set; }
        public ConcurrentDictionary<string, int> Subs { get; set; }
        public ConcurrentDictionary<string, int> Tags { get; set; }

        public ConcurrentDictionary<string, int> Nums { get; set; }
        public ConcurrentDictionary<string, int> Texts { get; set; }

        private readonly ILog log;
        protected override ILog Log
        {
            get
            {
                return log;
            }
        }
        public EdgarDatasetPresentationService()
        {
            log = log4net.LogManager.GetLogger(this.GetType().Name);
        }

        public override void Process(EdgarTaskState state, bool processInParallel, string fileToProcess, string fieldToUpdate)
        {
            //options:
            //a) reindex clustered and disable all non clustered and enable by the end
            //b) launch trhread that reindex every x time
            //c) ?
            

            try
            {
                Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
                Log.Info("Datasetid " + state.Dataset.Id.ToString() + " -- " + fileToProcess + " -- BEGIN process");
                if (!IsAlreadyProcessed(state.Dataset, fieldToUpdate))
                {
                    string cacheFolder = ConfigurationManager.AppSettings["cache_folder"];
                    string filepath = cacheFolder + state.Dataset.RelativePath.Replace("/", "\\").Replace(".zip", "") + "\\" + fileToProcess;
                    string[] allLines = File.ReadAllLines(filepath);
                    string header = allLines[0];

                    base.UpdateTotal(state, fieldToUpdate, allLines.Length - 1);
                    string rangeMsg = "Datasetid " + state.Dataset.Id.ToString() + " -- " + fileToProcess + " -- range: " +1+ " to " + allLines.Length;
                    ConcurrentDictionary<string, int> existing = this.GetAsConcurrent(state.Dataset.Id);
                    List<Exception> exceptions = new List<Exception>();
                    ConcurrentDictionary<int, string> failedLines = new ConcurrentDictionary<int, string>();

                    string fileToInsert = filepath + "_bulkinsert_" + DateTime.Now.ToString("yyyyMMddmmss") + ".tsv";

                    StreamWriter sw = File.CreateText(fileToInsert);

                    List<string> fieldNames = header.Split('\t').ToList();
                    try
                    {
                        for (int i = 1; i < allLines.Length; i++)
                        {
                            List<string> fields = allLines[i].Split('\t').ToList();
                            string adsh = fields[fieldNames.IndexOf("adsh")];
                            string line = fields[fieldNames.IndexOf("line")];
                            string report = fields[fieldNames.IndexOf("report")];
                            //select S.ADSH + CAST(p.ReportNumber as varchar) + cast(p.Line as varchar) [key] ...

                            string key = adsh + report + line;
                            if (!existing.ContainsKey(key))
                            {
                                try
                                {
                                    EdgarDatasetPresentation pre = Parse(null, fieldNames, fields, i + 1, existing);
                                    pre.DatasetId = state.Dataset.Id;
                                    string lineToInsert = ToLine(pre);
                                    sw.Write(lineToInsert);
                                }
                                catch (Exception ex)
                                {
                                    EdgarLineException elex = new EdgarLineException(fileToProcess, i, ex);
                                    exceptions.Add(elex);
                                    failedLines.TryAdd(i, line);
                                    Log.Error(rangeMsg + " -- line[" + i.ToString() + "]: " + allLines[i]);
                                    Log.Error(rangeMsg + " -- line[" + i.ToString() + "]: " + ex.Message, elex);
                                    if (exceptions.Count > MaxErrorsAllowed)
                                    {
                                        Log.Fatal(rangeMsg + " -- line[" + i.ToString() + "]: max errors allowed reached", ex);
                                        throw new EdgarDatasetException(fileToProcess, exceptions);
                                    }
                                }
                            }
                        }
                    }
                    finally
                    {
                        sw.Close();
                        WriteFailedLines(filepath, header, failedLines, allLines.Length);
                    }
                    using (IAnalystRepository repo = new AnalystRepository(new AnalystContext()))
                    {
                        repo.ExecuteBulkInsert("EdgarDatasetPresentations", fileToInsert, FIELD_SEPARATOR, LINE_SEPARATOR);
                    }
                }
                else
                {
                    Log.Info("Datasetid " + state.Dataset.Id.ToString() + " -- " + fileToProcess + " -- The complete file is already processed");
                }

                watch.Stop();
                TimeSpan ts = watch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                Log.Info("Datasetid " + state.Dataset.Id.ToString() + " -- " + fileToProcess + " -- time: " + elapsedTime);
                state.ResultOk = true;
            }
            catch (Exception ex)
            {
                state.ResultOk = false;
                state.Exception = new EdgarDatasetException(fileToProcess, ex);
            }

        }

        private string ToLine(EdgarDatasetPresentation pre)
        {
            return
                FIELD_SEPARATOR + //identity column remains empty
                pre.ReportNumber + FIELD_SEPARATOR +
                pre.Line + FIELD_SEPARATOR +
                pre.FinancialStatement + FIELD_SEPARATOR +
                (pre.Inpth?"1":"0") + FIELD_SEPARATOR +
                pre.PreferredLabelXBRLLinkRole + FIELD_SEPARATOR +
                pre.PreferredLabel + FIELD_SEPARATOR +
                (pre.Negating ? "1" : "0") + FIELD_SEPARATOR +
                pre.LineNumber + FIELD_SEPARATOR +
                pre.DatasetId + FIELD_SEPARATOR +
                pre.SubmissionId + FIELD_SEPARATOR +
                pre.TagId + FIELD_SEPARATOR +
                (pre.NumberId.HasValue? pre.NumberId.Value.ToString():"") + FIELD_SEPARATOR +
                (pre.TextId.HasValue? pre.TextId.Value.ToString():"") + FIELD_SEPARATOR +
                (pre.RenderId != 0 ? pre.RenderId.ToString():"" )+ FIELD_SEPARATOR +
                pre.ADSH_Tag_Version + LINE_SEPARATOR;
        }

        public override void Add(IAnalystRepository repo, EdgarDataset dataset, EdgarDatasetPresentation file)
        {
            if(file.Id <= 0)
                repo.Add(dataset,file);
        }

        public override EdgarDatasetPresentation Parse(IAnalystRepository repository, List<string> fieldNames, List<string> fields, int lineNumber, ConcurrentDictionary<string, int> existing)
        {
            /*
            adsh	report	line	stmt	inpth	rfile	tag	version	prole	plabel	negating
            0001163302-16-000148	1	4	CP	0	H	DocumentFiscalYearFocus	dei/2014	terseLabel	Document Fiscal Year Focus	0
            0001163302-16-000148	1	3	CP	0	H	DocumentPeriodEndDate	dei/2014	terseLabel	Document Period End Date	0
            0001125345-17-000041	6	1	CF	0	H	NetCashProvidedByUsedInOperatingActivitiesContinuingOperationsAbstract	us-gaap/2015	terseLabel	Operating activities	0
            0001104659-17-016575	111	6		0	H	NetCashProvidedByUsedInOperatingActivitiesContinuingOperationsAbstract	us-gaap/2015	terseLabel	Cash flows from operating activities:	0
            ...
            */
            try
            {
                EdgarDatasetPresentation pre;
                string adsh = fields[fieldNames.IndexOf("adsh")];
                string line = fields[fieldNames.IndexOf("line")];
                string report = fields[fieldNames.IndexOf("report")];
                //select S.ADSH + CAST(p.ReportNumber as varchar) + cast(p.Line as varchar) [key] ...

                string key = adsh + report + line;
                if (existing.ContainsKey(key))
                {
                    //Here, it should retrieve the entity from DB but it only saves it.
                    //pre = repository.Get<EdgarDatasetPresentation>(key);
                    pre = new EdgarDatasetPresentation();
                    pre.Id = existing[key];
                }
                else
                { 
                    pre = new EdgarDatasetPresentation();
                    pre.SubmissionId = Subs[adsh];
                    pre.ReportNumber = Convert.ToInt32(report);
                    pre.RenderId = Renders[adsh + report];
                    pre.Line = Convert.ToInt32(line);
                    pre.FinancialStatement = fields[fieldNames.IndexOf("stmt")];
                    if (string.IsNullOrEmpty(pre.FinancialStatement))
                        throw new Exception("Field FinancialStatement cannot be null or empty");
                    pre.Inpth = fields[fieldNames.IndexOf("inpth")] == "1";
                    pre.RenderFile = fields[fieldNames.IndexOf("rfile")][0];
                    string tag = fields[fieldNames.IndexOf("tag")];
                    string version = fields[fieldNames.IndexOf("version")];
                    pre.TagId = Tags[tag + version];
                    pre.PreferredLabelXBRLLinkRole = fields[fieldNames.IndexOf("prole")];
                    pre.PreferredLabel = fields[fieldNames.IndexOf("plabel")];
                    pre.Negating = !(fields[fieldNames.IndexOf("negating")] == "0");
                    pre.LineNumber = lineNumber;

                    string numKey = adsh + tag + version;
                    if (Nums.ContainsKey(numKey))
                        pre.NumberId = Nums[numKey];
                    else
                        pre.ADSH_Tag_Version = adsh + "|" + tag + "|" + version;

                    if (Texts.ContainsKey(numKey))
                        pre.TextId = Texts[numKey];
                    else
                        pre.ADSH_Tag_Version = adsh + "|" + tag + "|" + version;

                }
                return pre;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public override IList<EdgarTuple> GetKeys(IAnalystRepository repository, int datasetId)
        {
            return repository.GetPresentationsKeys(datasetId);
        }

        public override string GetKey(List<string> fieldNames, List<string> fields)
        {
            throw new NotImplementedException();
        }
    }
}
