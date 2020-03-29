using FinancialAnalyst.BatchProcesses.DB.EdgarSEC.Repositories;
using FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess;
using FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess.ParserStrategies.Interfaces;
using FinancialAnalyst.Common.Entities.EdgarSEC;
using FinancialAnalyst.Common.Entities.EdgarSEC.Datasets;
using FinancialAnalyst.Common.Entities.EdgarSEC.Repositories;
using FinancialAnalyst.Common.Exceptions;
using FinancialAnalyst.Common.Exceptions.EdgarSEC;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess
{
    public abstract class TemplateMethodOfEdgarDatasetParser<T> : ITemplateMethodOfEdgarDatasetParser<T> where T : class, IEdgarDatasetFile
    {
        private const int DEFAULT_MAX_ERRORS_ALLOWED = int.MaxValue;
        protected abstract log4net.ILog Log { get; }

        protected abstract DatasetsTables RelatedTable { get; }

        protected int MaxErrorsAllowed
        {
            get
            {
                string strValue = ConfigurationManager.AppSettings["maxerrorsallowed"];
                int iValue;
                if (int.TryParse(strValue, out iValue))
                    return iValue;
                else
                    return DEFAULT_MAX_ERRORS_ALLOWED;
            }
        }

        public ConcurrentDictionary<string, int> GetAsConcurrent(int datasetId)
        {
            ConcurrentDictionary<string, int> ret = new ConcurrentDictionary<string, int>();
            using (IEdgarDatasetsRepository repository = new EdgarRepository())
            {
                IList<EdgarTuple> keysId = GetKeys(repository, datasetId);
                foreach (EdgarTuple t in keysId)
                {
                    ret.TryAdd(t.Key, t.Id);
                }
            }
            return ret;
        }

        public virtual void Process(EdgarTaskState state, bool processInParallel, string fileToProcess, string fieldToUpdate)
        {
            try
            {
                Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
                Log.Info("Datasetid " + state.Dataset.Id.ToString() + " -- " + fileToProcess + " -- BEGIN PROCESS");
                int savedInDb;
                if (!IsAlreadyProcessed(state, fieldToUpdate, out savedInDb))
                {
                    string cacheFolder = ConfigurationManager.AppSettings["cache_folder"];
                    string tsvFileName = state.Dataset.RelativePath.Replace("/", "\\").Replace(".zip", "") + "\\" + fileToProcess;
                    string filepath = cacheFolder + tsvFileName;
                    string[] allLines = File.ReadAllLines(filepath);
                    string header = allLines[0];

                    UpdateTotalField(state, fieldToUpdate, allLines.Length - 1);

                    ConcurrentBag<int> missing;
                    if (savedInDb == 0)
                    {
                        missing = null;
                    }
                    else
                    {
                        missing = GetMissingLines(state.Dataset.Id, allLines.Length - 1);
                    }

                    ProcessFile(missing, fileToProcess, fieldToUpdate, state, allLines, header, cacheFolder, tsvFileName, processInParallel);

                    Log.Info("Datasetid " + state.Dataset.Id.ToString() + " -- " + fileToProcess + " -- Process finished, updating dataset status");
                    savedInDb = state.DatasetSharedRepo.GetCount<T>(state.Dataset.Id);
                    UpdateProcessedField(state, fieldToUpdate, savedInDb);
                }
                else
                {
                    state.FileNameToReprocess = null;
                    Log.Info("Datasetid " + state.Dataset.Id.ToString() + " -- " + fileToProcess + " -- The complete file is already processed");
                }

                watch.Stop();
                TimeSpan ts = watch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                Log.Info("Datasetid " + state.Dataset.Id.ToString() + " -- " + fileToProcess + " -- END PROCESS -- time: " + elapsedTime);
                state.ResultOk = true;
            }
            catch (Exception ex)
            {
                state.ResultOk = false;
                state.Exception = new EdgarDatasetException(fileToProcess, ex);
                Log.Fatal("Datasetid " + state.Dataset.Id.ToString() + " -- " + fileToProcess + " -- Error: " + ex.Message, ex);
            }
        }

        protected string WriteFailedLines(string folder, string fileName, string header, ConcurrentDictionary<int, string> failedLines, int totalLines)
        {
            string newFileName = null;
            if (failedLines.Count > 0)
            {
                newFileName = fileName + "_failed_" + DateTime.Now.ToString("yyyyMMddmmss") + ".tsv";
                StreamWriter sw = File.CreateText(folder + newFileName);
                //The first line is the header (line 0)
                //The second line is the firs row (line 1)
                sw.WriteLine(header);
                for (int i = 1; i <= totalLines; i++)
                {
                    int lineNumber = i + 1;
                    if (failedLines.ContainsKey(lineNumber))
                    {
                        sw.WriteLine(failedLines[lineNumber]);
                    }
                    else
                        sw.WriteLine("");
                }
                sw.Close();
            }
            return newFileName;
        }

        private ConcurrentBag<int> GetMissingLines(int datasetId, int totalLines)
        {
            List<int> missing;
            using (IEdgarDatasetsRepository repo = new EdgarRepository())
            {
                missing = repo.GetMissingLines(datasetId, RelatedTable, totalLines);
            }
            ConcurrentBag<int> bag = new ConcurrentBag<int>(missing);
            return bag;
        }

        private bool IsAlreadyProcessed(EdgarTaskState state, string fieldToUpdate, out int savedInDb)
        {
            using (IEdgarDatasetsRepository repo = new EdgarRepository())
            {
                savedInDb = repo.GetCount<T>(state.Dataset.Id);
                int processed = (int)state.Dataset.GetType().GetProperty("Processed" + fieldToUpdate).GetValue(state.Dataset);
                if (savedInDb != processed)
                    UpdateProcessedField(state, fieldToUpdate, savedInDb);
                int total = (int)state.Dataset.GetType().GetProperty("Total" + fieldToUpdate).GetValue(state.Dataset);
                return savedInDb == processed && processed == total && total != 0;
            }
        }

        private void UpdateProcessedField(EdgarTaskState state, string fieldToUpdate, int value)
        {
            UpdateField("Processed", fieldToUpdate, state, value);
        }

        private void UpdateTotalField(EdgarTaskState state, string fieldToUpdate, int value)
        {
            UpdateField("Total", fieldToUpdate, state, value);
        }

        private void UpdateField(string prefix, string fieldToUpdate, EdgarTaskState state, int value)
        {
            string field = prefix + fieldToUpdate;
            int currentValue = (int)state.Dataset.GetType().GetProperty(field).GetValue(state.Dataset);
            state.Dataset.GetType().GetProperty(field).SetValue(state.Dataset, value);
            state.DatasetSharedRepo.UpdateEdgarDataset(state.Dataset, field);
        }

        #region Abstract methods

        public abstract void ProcessFile(ConcurrentBag<int> missing, string fileToProcess, string fieldToUpdate, EdgarTaskState state, string[] allLines, string header, string cacheFolder, string tsvFileName, bool processInParallel);

        public abstract IList<EdgarTuple> GetKeys(IEdgarDatasetsRepository repository, int datasetId);

        #endregion

        public void Dispose()
        {

        }

    }
}
