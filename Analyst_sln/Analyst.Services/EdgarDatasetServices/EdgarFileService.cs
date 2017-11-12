using Analyst.DBAccess.Contexts;
using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Analyst.Services.EdgarDatasetServices
{
    public interface IEdgarFileService<T> where T : IEdgarDatasetFile
    {
        ConcurrentDictionary<string, T> GetAsConcurrent();
        ConcurrentDictionary<string, T> GetAsConcurrent(string include);
        void Process(EdgarTaskState state, bool processInParallel, string fileToProcess, string fieldToUpdate);
    }

    public abstract class EdgarFileService<T>:IEdgarFileService<T> where T:IEdgarDatasetFile
    {

        public ConcurrentDictionary<string, T> GetAsConcurrent()
        {
            ConcurrentDictionary<string, T> ret = new ConcurrentDictionary<string, T>();
            IAnalystRepository repository = new AnalystRepository(new AnalystContext());
            
            IList<T> xs = repository.Get<T>();
            foreach (T x in xs)
            {
                ret.TryAdd(x.Key, x);
            }
            return ret;
        }

        public ConcurrentDictionary<string, T> GetAsConcurrent(string include)
        {
            ConcurrentDictionary<string, T> ret = new ConcurrentDictionary<string, T>();
            IAnalystRepository repository = new AnalystRepository(new AnalystContext());

            IList<T> xs = repository.Get<T>(include);
            foreach (T x in xs)
            {
                ret.TryAdd(x.Key, x);
            }
            return ret;
        }

        public void Process(EdgarTaskState state,bool processInParallel, string fileToProcess,string fieldToUpdate)
        {
            try
            {
                if (IsAlreadyProcessed(state.Dataset,fieldToUpdate))
                    return;
                string cacheFolder = ConfigurationManager.AppSettings["cache_folder"];
                string filepath = cacheFolder + state.Dataset.RelativePath.Replace("/", "\\").Replace(".zip", "") + "\\" + fileToProcess;
                string[] allLines = File.ReadAllLines(filepath);
                string header = allLines[0];
                string field = "Total" + fieldToUpdate;
                state.Dataset.GetType().GetProperty(field).SetValue(state.Dataset,allLines.Length - 1);
                state.DatasetSharedRepo.UpdateEdgarDataset(state.Dataset,field);
                if (processInParallel)
                {
                    //https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/custom-partitioners-for-plinq-and-tpl?view=netframework-4.5.2

                    // Partition the entire source array.
                    OrderablePartitioner<Tuple<int, int>> rangePartitioner = Partitioner.Create(1, allLines.Length);

                    // Loop over the partitions in parallel.
                    Parallel.ForEach(rangePartitioner, (range, loopState) =>
                    {
                        ProcessRange(state, range, allLines, header);
                    });
                }
                else
                {
                    ProcessRange(state, new Tuple<int, int>(1, allLines.Length), allLines, header);
                }

                state.ResultOk = true;
            }
            catch (Exception ex)
            {
                state.ResultOk = false;
                state.Exception = ex;
            }
        }

        private bool IsAlreadyProcessed(EdgarDataset ds, string fieldToUpdate)
        {
            using (IAnalystRepository repo = new AnalystRepository(new AnalystContext()))
            {
                int savedInDb = repo.GetCount<T>();
                int processed = (int)ds.GetType().GetProperty("Processed" + fieldToUpdate).GetValue(ds);
                int total = (int)ds.GetType().GetProperty("Total" + fieldToUpdate).GetValue(ds);
                return savedInDb == processed && processed == total && total != 0;
            }
        }

        protected void ProcessRange(EdgarTaskState state, Tuple<int, int> range, string[] allLines, string header)
        {
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
                    for (int i = range.Item1; i < range.Item2; i++)
                    {
                        string line = allLines[i];
                        List<string> fields = line.Split('\t').ToList();
                        T file = Parse(repo, fieldNames,fields, i);
                        Add(repo, state.Dataset, file);
                    }
                }
                finally
                {
                    repo.ContextConfigurationAutoDetectChangesEnabled = true;
                }
            }
        }

        public abstract void Add(IAnalystRepository repo, EdgarDataset dataset, T file);
        public abstract T Parse(IAnalystRepository repository, List<string> fieldNames, List<string> fields, int lineNumber);
        
    }
}