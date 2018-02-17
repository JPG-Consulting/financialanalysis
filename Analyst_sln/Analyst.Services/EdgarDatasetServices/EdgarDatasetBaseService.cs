using Analyst.DBAccess.Contexts;
using Analyst.Domain.Edgar;
using Analyst.Domain.Edgar.Datasets;
using Analyst.Domain.Edgar.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Analyst.Services.EdgarDatasetServices
{


    public interface IEdgarDatasetBaseService<T>: IDisposable where T :class, IEdgarDatasetFile
    {
        ConcurrentDictionary<string, int> GetAsConcurrent(int datasetId);
        void Process(EdgarTaskState state,bool processBulk, bool processInParallel, string fileToProcess, string fieldToUpdate);
       
    }

    public abstract class EdgarDatasetBaseService<T>: IEdgarDatasetBaseService<T> where T:class,IEdgarDatasetFile
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
            using (IAnalystRepository repository = new AnalystRepository(new AnalystContext()))
            {
                IList<EdgarTuple> keysId = GetKeys(repository, datasetId);
                foreach (EdgarTuple t in keysId)
                {
                    ret.TryAdd(t.Key, t.Id);
                }
            }
            return ret;
        }

 

        public virtual void Process(EdgarTaskState state, bool processBulk, bool processInParallel, string fileToProcess,string fieldToUpdate)
        {
            try
            {
                Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
                Log.Info("Datasetid " + state.Dataset.Id.ToString() + " -- " + fileToProcess + " -- BEGIN PROCESS");
                int savedInDb;
                if (!IsAlreadyProcessed(state, fieldToUpdate,out savedInDb))
                {
                    string cacheFolder = ConfigurationManager.AppSettings["cache_folder"];
                    string tsvFileName = state.Dataset.RelativePath.Replace("/", "\\").Replace(".zip", "") + "\\" + fileToProcess;
                    string filepath = cacheFolder + tsvFileName;
                    string[] allLines = File.ReadAllLines(filepath);
                    string header = allLines[0];

                    UpdateTotalField(state,fieldToUpdate, allLines.Length - 1);

                    ConcurrentBag<int> missing;
                    if (savedInDb == 0)
                    {
                        missing = null;
                    }
                    else
                    {
                        missing = GetMissingLines(state.Dataset.Id, allLines.Length - 1);
                    }

                    if (processBulk)
                        ProcessBulk(missing,fileToProcess,fieldToUpdate, state, allLines, header);
                    else
                        ProcessLineByLine(missing,fileToProcess, fieldToUpdate, state, allLines, header,cacheFolder,tsvFileName, processInParallel);
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
                state.Exception = new EdgarDatasetException(fileToProcess,ex);
                Log.Fatal("Datasetid " + state.Dataset.Id.ToString() + " -- " + fileToProcess + " -- Error: " + ex.Message, ex);
            }
        }

        public void ProcessLineByLine(ConcurrentBag<int> missing, string fileToProcess, string fieldToUpdate, EdgarTaskState state, string[] allLines,string header, string cacheFolder, string tsvFileName,bool processInParallel)
        {

            ConcurrentDictionary<int, string> failedLines = new ConcurrentDictionary<int, string>();
            if (processInParallel && allLines.Length > 1)
            {
                //https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/custom-partitioners-for-plinq-and-tpl?view=netframework-4.5.2

                // Partition the entire source array.
                OrderablePartitioner<Tuple<int, int>> rangePartitioner = Partitioner.Create(1, allLines.Length);

                // Loop over the partitions in parallel.
                Parallel.ForEach(rangePartitioner, (range, loopState) =>
                {
                    ProcessRange(fileToProcess, state, range, allLines, header, missing, failedLines);
                });

            }
            else
            {
                ProcessRange(fileToProcess, state, new Tuple<int, int>(1, allLines.Length), allLines, header, missing, failedLines);
            }
            state.FileNameToReprocess = WriteFailedLines(cacheFolder, tsvFileName, header, failedLines, allLines.Length);

        }

        public ConcurrentBag<int> GetMissingLines(int datasetId, int totalLines)
        {
            List<int> missing;
            using (IAnalystRepository repo = new AnalystRepository(new AnalystContext()))
            {
                missing = GetMissingLinesByTable(repo,datasetId, totalLines);
            }
            ConcurrentBag<int> bag = new ConcurrentBag<int>(missing);
            return bag;
        }

        private void ProcessBulk(ConcurrentBag<int> missing,string fileToProcess,string fieldToUpdate, EdgarTaskState state, string[] allLines, string header)
        {
            //https://msdn.microsoft.com/en-us/library/ex21zs8x(v=vs.110).aspx
            //https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/transaction-and-bulk-copy-operations

            Log.Info("Datasetid " + state.Dataset.Id.ToString() + " -- " + fileToProcess + " -- BEGIN BULK PROCESS");
            using (SQLAnalystRepository repo = new SQLAnalystRepository())
            {
                Log.Info("Datasetid " + state.Dataset.Id.ToString() + " -- " + fileToProcess + " -- Retrieving structure");
                DataTable dt = GetEmptyDataTable(repo);
                List<string> fieldNames = header.Split('\t').ToList();

                Log.Info("Datasetid " + state.Dataset.Id.ToString() + " -- " + fileToProcess + " -- Creating DataTable");
                //first line is the header
                for (int i=1;i<allLines.Length;i++)
                {
                    if (missing == null || missing.Contains(i + 1))
                    {
                        string line = allLines[i];
                        if (!string.IsNullOrEmpty(line))
                        {
                            List<string> fields = line.Split('\t').ToList();
                            DataRow dr = dt.NewRow();
                            Parse(fieldNames, fields, i + 1, dr, state.Dataset.Id);
                            dt.Rows.Add(dr);
                        }
                    }
                }
                Log.Info("Datasetid " + state.Dataset.Id.ToString() + " -- " + fileToProcess + " -- Starting bulk copy");
                BulkCopy(repo, dt);
                Log.Info("Datasetid " + state.Dataset.Id.ToString() + " -- " + fileToProcess + " -- Bulk copy finished, updating dataset status");
                UpdateProcessedField(state, fieldToUpdate, dt.Rows.Count);
                Log.Info("Datasetid " + state.Dataset.Id.ToString() + " -- " + fileToProcess + " -- END BULK PROCESS");
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
            if (currentValue == 0)
            {
                state.Dataset.GetType().GetProperty(field).SetValue(state.Dataset, value);
                state.DatasetSharedRepo.UpdateEdgarDataset(state.Dataset, field);
            }
        }


        private string WriteFailedLines(string folder,string fileName, string header, ConcurrentDictionary<int,string> failedLines,int totalLines)
        {
            string newFileName = null;
            if(failedLines.Count > 0)
            {
                newFileName = fileName + "_failed_" + DateTime.Now.ToString("yyyyMMddmmss") + ".tsv";
                StreamWriter sw = File.CreateText(folder + newFileName);
                //The first line is the header (line 0)
                //The second line is the firs row (line 1)
                sw.WriteLine(header);
                for(int i=1;i<=totalLines; i++)
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

        private bool IsAlreadyProcessed(EdgarTaskState state, string fieldToUpdate, out int savedInDb)
        {
            using (IAnalystRepository repo = new AnalystRepository(new AnalystContext()))
            {
                savedInDb = repo.GetCount<T>(state.Dataset.Id);
                int processed = (int)state.Dataset.GetType().GetProperty("Processed" + fieldToUpdate).GetValue(state.Dataset);
                if (savedInDb != processed)
                    UpdateProcessedField(state, fieldToUpdate, savedInDb);
                int total = (int)state.Dataset.GetType().GetProperty("Total" + fieldToUpdate).GetValue(state.Dataset);
                return savedInDb == processed && processed == total && total != 0;
            }
        }

        protected void ProcessRange(string fileName,EdgarTaskState state, Tuple<int, int> range, string[] allLines, string header,ConcurrentBag<int> missing, ConcurrentDictionary<int,string> failedLines)
        {
            Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
            string rangeMsg = "Datasetid " + state.Dataset.Id.ToString() + " -- " + fileName + " -- range: " + range.Item1 + " to " + range.Item2;
            Log.Info(rangeMsg + " -- BEGIN");

            /*
            EF isn't thread safe and it doesn't allow parallel
            https://stackoverflow.com/questions/12827599/parallel-doesnt-work-with-entity-framework
            https://stackoverflow.com/questions/9099359/entity-framework-and-multi-threading
            https://social.msdn.microsoft.com/Forums/en-US/e5cb847c-1d77-4cd0-abb7-b61890d99fae/multithreading-and-the-entity-framework?forum=adodotnetentityframework
            solution: only 1 context for the entiry partition --> works
            */
            using (IAnalystRepository repo = new AnalystRepository(new AnalystContext()))
            {
                //It improves performance
                //https://msdn.microsoft.com/en-us/library/jj556205(v=vs.113).aspx
                repo.ContextConfigurationAutoDetectChangesEnabled = false;
                try
                {
                    List<string> fieldNames = header.Split('\t').ToList();
                    List<Exception> exceptions = new List<Exception>();
                    string line = null;
                    int lineNumber = 0;
                    for (int i = range.Item1; i < range.Item2; i++)
                    {
                        try
                        {
                            lineNumber = i + 1;//i+1: indexes starts with 0 but header is line 1 and the first row is line 2
                            //It will be processed if:
                            //it's the first time (missing == null) 
                            //or it's processed again and line wasn't processed the firs time (missing.Contains(i+1))
                            if (missing == null || missing.Contains(i+1))
                            {
                                Log.Debug(rangeMsg + " -- parsing[" + i.ToString() + "]: " + line);
                                line = allLines[i];
                                if (!string.IsNullOrEmpty(line))//files with error lines has an empty line for processed lines
                                {

                                    List<string> fields = line.Split('\t').ToList();
                                    T file = Parse(repo, fieldNames, fields, lineNumber);
                                    Add(repo, state.Dataset, file);
                                }
                            }

                        }
                        catch(Exception ex)
                        {
                            EdgarLineException elex = new EdgarLineException(fileName, lineNumber, ex);
                            exceptions.Add(elex);
                            failedLines.TryAdd(lineNumber, line);
                            Log.Error(rangeMsg + " -- line[" + lineNumber.ToString() + "]: " + line);
                            Log.Error(rangeMsg + " -- line[" + lineNumber.ToString() + "]: " + ex.Message, elex);
                            if (exceptions.Count > MaxErrorsAllowed)
                            {
                                Log.Fatal(rangeMsg + " -- line[" + i.ToString() + "]: max errors allowed reached", ex);
                                throw new EdgarDatasetException(fileName, exceptions);
                            }
                            
                        }
                    }
                    

                }
                finally
                {
                    repo.ContextConfigurationAutoDetectChangesEnabled = true;
                }
            }
            watch.Stop();
            TimeSpan ts = watch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Log.Info(rangeMsg + " -- END");
            Log.Info(rangeMsg + " -- time: " + elapsedTime);

        }

        public abstract void Add(IAnalystRepository repo, EdgarDataset dataset, T file);

        public abstract T Parse(IAnalystRepository repository, List<string> fieldNames, List<string> fields, int lineNumber);
     
        public abstract IList<EdgarTuple> GetKeys(IAnalystRepository repository, int datasetId);

        public abstract string GetKey(List<string> fieldNames, List<string> fields);

        public abstract void BulkCopy(SQLAnalystRepository repo, DataTable dt);

        public abstract DataTable GetEmptyDataTable(SQLAnalystRepository repo);

        public abstract void Parse(List<string> fieldNames, List<string> fields, int lineNumber, DataRow dr, int edgarDatasetId);



        public abstract List<int> GetMissingLinesByTable(IAnalystRepository repo, int datasetId, int totalLines);
        

        public void Dispose()
        {

        }
    }
}