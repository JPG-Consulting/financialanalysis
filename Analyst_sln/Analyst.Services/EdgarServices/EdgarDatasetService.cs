using Analyst.DBAccess.Contexts;
using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Services.EdgarServices
{

    public interface IEdgarDatasetService
    {
        IList<EdgarDataset> GetDatasets();
        void ProcessDataset(int id);
        EdgarDataset GetDataset(int id);
    }
    public class EdgarDatasetService: EdgarFileService<EdgarDataset>, IEdgarDatasetService, IDisposable
    {
        private static ConcurrentDictionary<int, Task> datasetsInProcess = new ConcurrentDictionary<int,Task>();

        private IAnalystRepository repository;
        private ISubmissionService submissionService;
        private ITagService tagService;
        private INumService numService;
        private IDimensionService dimensionService;
        public EdgarDatasetService(IAnalystRepository repository, ISubmissionService submissionService, ITagService tagService,INumService numService, IDimensionService dimensionService)
        {
            this.repository = repository;
            this.submissionService = submissionService;
            this.tagService = tagService;
            this.numService = numService;
            this.dimensionService = dimensionService;
        }

        public IList<EdgarDataset> GetDatasets()
        {
            IList<EdgarDataset> datasets = repository.Get<EdgarDataset>();
            return datasets;
        }

        public EdgarDataset GetDataset(int id)
        {
            EdgarDataset ds = repository.GetDataset(id);
            return ds;
        }

        public void ProcessDataset(int id)
        {
            //https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/task-based-asynchronous-programming?view=netframework-4.5.2

            if (datasetsInProcess.ContainsKey(id))
            {
                Task t = datasetsInProcess[id];
                if (t.Status != TaskStatus.Running)
                    t.Dispose();
            }
            Run(id);
        }

        private void Run(int id)
        {
            Task t = new Task(() =>
            {
                EdgarDataset ds = repository.GetDataset(id);
                EdgarTaskState[] states = LoadNumRelatedData(ds, repository);
                for (int i = 0; i < states.Length; i++)
                {
                    if (states[i].ResultOk.HasValue && !states[i].ResultOk.Value)
                        throw new ApplicationException("Error in process X", states[i].Exception);
                }
                LoadNums(ds, repository);
            });
            t.Start();
            datasetsInProcess.TryAdd(id, t);
        }

        private EdgarTaskState[] LoadNumRelatedData(EdgarDataset ds,IAnalystRepository repo)
        {
            int taskAmount = 3;
            EdgarTaskState[] states = new EdgarTaskState[taskAmount];
            states[0] = new EdgarTaskState(ds, repo);
            states[1] = new EdgarTaskState(ds, repo);
            states[2] = new EdgarTaskState(ds, repo);
            Task[] taskArray = new Task[taskAmount];
            taskArray[0] = Task.Factory.StartNew(() => submissionService.ProcessSubmissions(states[0]));
            taskArray[1] = Task.Factory.StartNew(() => tagService.ProcessTags(states[1]));
            taskArray[2] = Task.Factory.StartNew(() => dimensionService.ProcessDimensions(states[2]));
            Task.WaitAll(taskArray);
            return states;
        }

        private void LoadNums(EdgarDataset ds,IAnalystRepository repo)
        {
            EdgarTaskState state = new EdgarTaskState(ds, repo);
            ConcurrentDictionary<string, EdgarDatasetSubmissions> subs = submissionService.GetAsConcurrent();
            ConcurrentDictionary<string, EdgarDatasetTag> tags = tagService.GetAsConcurrent();
            ConcurrentDictionary<string, EdgarDatasetDimension> dims = dimensionService.GetAsConcurrent();

            numService.ProcessNums(state, subs, tags, dims);
        }

        public void Dispose()
        {
            //https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/task-cancellation?view=netframework-4.5.2
            //TODO: Cancel all tasks
            foreach (Task process in datasetsInProcess.Values.ToList())
            {
                
            }
        }
    }
}
