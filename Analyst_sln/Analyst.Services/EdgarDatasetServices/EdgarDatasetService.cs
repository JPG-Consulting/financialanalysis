using Analyst.DBAccess.Contexts;
using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Services.EdgarDatasetServices
{

    public interface IEdgarDatasetService
    {
        IList<EdgarDataset> GetDatasets();
        void ProcessDataset(int id);
        EdgarDataset GetDataset(int id);
    }
    public class EdgarDatasetService: IEdgarDatasetService, IDisposable
    {
        private static ConcurrentDictionary<int, Task> datasetsInProcess = new ConcurrentDictionary<int,Task>();

        private IAnalystRepository repository;
        private IEdgarDatasetSubmissionsService submissionService;
        private IEdgarDatasetTagService tagService;
        private IEdgarDatasetNumService numService;
        private IEdgarDatasetDimensionService dimensionService;
        public EdgarDatasetService(IAnalystRepository repository, IEdgarDatasetSubmissionsService submissionService, IEdgarDatasetTagService tagService, IEdgarDatasetNumService numService, IEdgarDatasetDimensionService dimensionService)
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
                ManageErrors(states);
                LoadNums(ds, repository);
            });
            t.Start();
            datasetsInProcess.TryAdd(id, t);
        }
        private EdgarTaskState[] LoadNumRelatedData(EdgarDataset ds, IAnalystRepository repo)
        {
            Dictionary<int, EdgarTaskState> states = new Dictionary<int, EdgarTaskState>();
            states.Add(0, new EdgarTaskState(ds, repo));
            states.Add(1, new EdgarTaskState(ds, repo));
            states.Add(2, new EdgarTaskState(ds, repo));
            Dictionary<int, Task> tasks = new Dictionary<int, Task>();
            tasks.Add(0, Task.Factory.StartNew(() => submissionService.Process(states[0], false, EdgarDatasetSubmissions.FILE_NAME, "Submissions")));
            tasks.Add(1,Task.Factory.StartNew(() => tagService.Process(states[1],true,EdgarDatasetTag.FILE_NAME,"Tags")));
            tasks.Add(2,Task.Factory.StartNew(() => dimensionService.Process(states[2],true,EdgarDatasetDimension.FILE_NAME,"Dimensions")));
            Task.WaitAll(tasks.Values.ToArray());
            return states.Values.ToArray();
        }


        private void ManageErrors(EdgarTaskState[] states)
        {
            EdgarDatasetException edex = null;
            for (int i = 0; i < states.Length; i++)
            {
                if (states[i].ResultOk.HasValue && !states[i].ResultOk.Value)
                {
                    if (edex == null)
                        edex = new EdgarDatasetException();
                    edex.AddInnerException(states[i].Exception);
                }
            }
            if (edex != null)
            {
                //reset status   
                repository.Dispose();
                repository = new AnalystRepository(new AnalystContext());
                throw edex;
            }
        }

        
        private void LoadNums(EdgarDataset ds,IAnalystRepository repo)
        {
            EdgarTaskState state = new EdgarTaskState(ds, repo);
            numService.Submissions = submissionService.GetAsConcurrent();
            numService.Tags = tagService.GetAsConcurrent();
            numService.Dimensions = dimensionService.GetAsConcurrent();

            numService.Process(state,true,EdgarDatasetNumber.FILE_NAME,"Numbers");
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
