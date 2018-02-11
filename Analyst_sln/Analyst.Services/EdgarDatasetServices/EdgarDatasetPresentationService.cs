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
using Analyst.Domain.Edgar.Exceptions;
using System.Data;

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
        public override void Add(IAnalystRepository repo, EdgarDataset dataset, EdgarDatasetPresentation file)
        {
            repo.Add(dataset,file);
        }

        public override EdgarDatasetPresentation Parse(IAnalystRepository repository, List<string> fieldNames, List<string> fields, int lineNumber)
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
                //string key = adsh + report + line;

                pre = new EdgarDatasetPresentation();
                pre.SubmissionId = Subs[adsh];
                pre.ReportNumber = Convert.ToInt32(report);
                pre.RenderId = Renders[adsh + report];
                pre.Line = Convert.ToInt32(line);
                pre.FinancialStatement = fields[fieldNames.IndexOf("stmt")];
                pre.Inpth = fields[fieldNames.IndexOf("inpth")] == "1";
                pre.RenderFile = fields[fieldNames.IndexOf("rfile")][0];
                pre.RenderFileStr = fields[fieldNames.IndexOf("rfile")];
                string tag = fields[fieldNames.IndexOf("tag")];
                string version = fields[fieldNames.IndexOf("version")];
                if(Tags.ContainsKey(tag+version))
                    pre.TagId = Tags[tag + version];
                else
                {
                    string tag2 = Encoding.GetEncoding(1252).GetString(Encoding.GetEncoding("iso-8859-7").GetBytes(tag));
                    if (Tags.ContainsKey(tag + version))
                        pre.TagId = Tags[tag + version];
                    else
                        throw new EdgarLineException(EdgarDatasetPresentation.FILE_NAME, lineNumber, "Error retrieving key: " + tag + version);
                }
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
        
        public void Process2(EdgarTaskState state, bool processBulk,bool processInParallel, string fileToProcess, string fieldToUpdate)
        {
            using (IAnalystRepository repo = new AnalystRepository(new AnalystContext()))
            {
                repo.EnablePresentationIndexes(false);
            }
            base.Process(state,processBulk, processInParallel, fileToProcess, fieldToUpdate);
            using (IAnalystRepository repo = new AnalystRepository(new AnalystContext()))
            {
                repo.EnablePresentationIndexes(true);
            }
        }

        public override void Parse(List<string> fieldNames, List<string> fields, int lineNumber, DataRow dr, int edgarDatasetId)
        {
            /*
            adsh	                report	line	stmt	inpth	rfile	tag	version	prole	plabel	negating
            0001163302-16-000148	1	    4	    CP	    0	    H	    DocumentFiscalYearFocus	dei/2014	terseLabel	Document Fiscal Year Focus	0
            0001163302-16-000148	1	    3	    CP	    0	    H	    DocumentPeriodEndDate	dei/2014	terseLabel	Document Period End Date	0
            0001125345-17-000041	6	    1	    CF	    0	    H	    NetCashProvidedByUsedInOperatingActivitiesContinuingOperationsAbstract	us-gaap/2015	terseLabel	Operating activities	0
            0001104659-17-016575	111	    6		        0	    H	    NetCashProvidedByUsedInOperatingActivitiesContinuingOperationsAbstract	us-gaap/2015	terseLabel	Cash flows from operating activities:	0
            ...
            */
            try
            {

                string adsh = fields[fieldNames.IndexOf("adsh")];
                string line = fields[fieldNames.IndexOf("line")];
                string report = fields[fieldNames.IndexOf("report")];
                dr["SubmissionId"] = Subs[adsh];
                dr["ReportNumber"] = Convert.ToInt32(report);
                dr["RenderId"] = Renders[adsh + report];
                dr["Line"] = Convert.ToInt32(line);
                dr["FinancialStatement"] = fields[fieldNames.IndexOf("stmt")];
                dr["Inpth"] = fields[fieldNames.IndexOf("inpth")] == "1";
                dr["RenderFileStr"] = fields[fieldNames.IndexOf("rfile")][0];
                string tag = fields[fieldNames.IndexOf("tag")];
                string version = fields[fieldNames.IndexOf("version")];
                if (Tags.ContainsKey(tag + version))
                dr["TagId"] = Tags[tag + version];
                else
                {
                    string tag2 = Encoding.GetEncoding(1252).GetString(Encoding.GetEncoding("iso-8859-7").GetBytes(tag));
                    if (Tags.ContainsKey(tag + version))
                        dr["TagId"] = Tags[tag + version];
                    else
                        throw new EdgarLineException(EdgarDatasetPresentation.FILE_NAME, lineNumber, "Error retrieving key: " + tag + version);
                }
                dr["PreferredLabelXBRLLinkRole"] = fields[fieldNames.IndexOf("prole")];
                dr["PreferredLabel"] = fields[fieldNames.IndexOf("plabel")];
                dr["Negating"] = !(fields[fieldNames.IndexOf("negating")] == "0");
                dr["DatasetId"] = edgarDatasetId;
                dr["LineNumber"] = lineNumber;


                string numKey = adsh + tag + version;
                if (Nums.ContainsKey(numKey))
                    dr["NumberId"] = Nums[numKey];
                else
                    dr["ADSH_Tag_Version"] = adsh + " | " + tag + "|" + version;

                if (Texts.ContainsKey(numKey))
                    dr["TextId"] = Texts[numKey];
                else
                    dr["ADSH_Tag_Version"] = adsh + " | " + tag + "|" + version;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void BulkCopy(SQLAnalystRepository repo, DataTable dt)
        {
            repo.BulkCopyPresentations(dt);
        }

        public override DataTable GetEmptyDataTable(SQLAnalystRepository repo)
        {
            return repo.GetEmptyPresentationDataTable();
        }
        public override List<int> GetMissingLinesByTable(IAnalystRepository repo, int datasetId, int totalLines)
        {
            return repo.GetMissingLines(datasetId,"EdgarDatasetPresentations", totalLines);
        }

    }
}
