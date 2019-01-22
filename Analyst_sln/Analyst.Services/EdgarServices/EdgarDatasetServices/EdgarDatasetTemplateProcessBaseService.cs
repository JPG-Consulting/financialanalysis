using Analyst.DBAccess.Contexts;
using Analyst.DBAccess.Repositories;
using Analyst.Domain.Edgar;
using Analyst.Domain.Edgar.Datasets;
using Analyst.Services.EdgarDatasetServices;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Services.EdgarServices.EdgarDatasetServices
{
    public interface IEdgarDatasetTemplateProcessBaseService<T> : IDisposable where T : class, IEdgarDatasetFile
    {
        ConcurrentDictionary<string, int> GetAsConcurrent(int datasetId);
        void Process(EdgarTaskState state, bool processInParallel, string fileToProcess, string fieldToUpdate);
    }

    public abstract class EdgarDatasetTemplateProcessBaseService<T>: IEdgarDatasetTemplateProcessBaseService<T> where T : class, IEdgarDatasetFile
    {
        private const int DEFAULT_MAX_ERRORS_ALLOWED = int.MaxValue;
        protected abstract log4net.ILog Log { get; }

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
            using (IAnalystEdgarDatasetsRepository repository = new AnalystEdgarDatasetsEFRepository())
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

        private ConcurrentBag<int> GetMissingLines(int datasetId, int totalLines)
        {
            List<int> missing;
            using (IAnalystEdgarDatasetsRepository repo = new AnalystEdgarDatasetsEFRepository())
            {
                missing = GetMissingLinesByTable(repo, datasetId, totalLines);
            }
            ConcurrentBag<int> bag = new ConcurrentBag<int>(missing);
            return bag;
        }

        private bool IsAlreadyProcessed(EdgarTaskState state, string fieldToUpdate, out int savedInDb)
        {
            using (IAnalystEdgarDatasetsRepository repo = new AnalystEdgarDatasetsEFRepository())
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

        public abstract IList<EdgarTuple> GetKeys(IAnalystEdgarDatasetsRepository repository, int datasetId);

        public abstract List<int> GetMissingLinesByTable(IAnalystEdgarDatasetsRepository repo, int datasetId, int totalLines);
        #endregion

        public void Dispose()
        {

        }

    }
}
