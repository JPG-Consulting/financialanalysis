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
        private IEdgarDatasetRenderingService renderingService;
        private IEdgarDatasetPresentationService presentationService;
        public EdgarDatasetService(IAnalystRepository repository, IEdgarDatasetSubmissionsService submissionService, IEdgarDatasetTagService tagService, IEdgarDatasetNumService numService, IEdgarDatasetDimensionService dimensionService, IEdgarDatasetRenderingService renderingService,IEdgarDatasetPresentationService presentationService)
        {
            this.repository = repository;
            this.submissionService = submissionService;
            this.tagService = tagService;
            this.numService = numService;
            this.dimensionService = dimensionService;
            this.renderingService = renderingService;
            this.presentationService = presentationService;
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
                EdgarTaskState[] states = LoadCoreData(ds, repository);
                ManageErrors(states);
                ConcurrentDictionary<string, EdgarDatasetTag> tags = tagService.GetAsConcurrent();
                ConcurrentDictionary<string, EdgarDatasetSubmission> subs = submissionService.GetAsConcurrent();
                states = LoadData(ds, repository,subs,tags);
                ManageErrors(states);

                //TODO: pending to process txt.tsv file (Plain Text)
                //TODO: pending to process cal.tsv (Calculations)
            });
            t.Start();
            datasetsInProcess.TryAdd(id, t);
        }
                
        private EdgarTaskState[] LoadCoreData(EdgarDataset ds, IAnalystRepository repo)
        {
            List<EdgarTaskState> states = new List<EdgarTaskState>();
            EdgarTaskState stateSubs, stateTag, stateDim;
            stateSubs = new EdgarTaskState(ds, repo);
            stateTag = new EdgarTaskState(ds, repo);
            stateDim = new EdgarTaskState(ds, repo);
            states.Add(stateSubs);
            states.Add(stateTag);
            states.Add(stateDim);
            IList<Task> tasks = new List<Task>();
            tasks.Add(Task.Factory.StartNew(() => submissionService.Process(stateSubs, false, EdgarDatasetSubmission.FILE_NAME, "Submissions")));
            tasks.Add(Task.Factory.StartNew(() => tagService.Process(stateTag,true,EdgarDatasetTag.FILE_NAME,"Tags")));
            tasks.Add(Task.Factory.StartNew(() => dimensionService.Process(stateDim,true,EdgarDatasetDimension.FILE_NAME,"Dimensions")));
            Task.WaitAll(tasks.ToArray());
            return states.ToArray();
        }

        

        private EdgarTaskState[] LoadData(EdgarDataset ds,IAnalystRepository repo,ConcurrentDictionary<string,EdgarDatasetSubmission> subs,ConcurrentDictionary<string,EdgarDatasetTag> tags)
        {
            List<EdgarTaskState> states = new List<EdgarTaskState>();
            List<Task> tasks = new List<Task>();

            //Process presentation file
            EdgarTaskState statePre = new EdgarTaskState(ds, repo);
            states.Add(statePre);
            EdgarTaskState stateRen = new EdgarTaskState(ds, repo);
            states.Add(stateRen);
            tasks.Add(
                Task.Factory.StartNew(
                    () =>
                        {
                            renderingService.Subs = subs;
                            renderingService.Process(stateRen, false, EdgarDatasetRendering.FILE_NAME, "Renders");
                            presentationService.Subs = subs;
                            presentationService.Tags = tags;
                            presentationService.Renders = renderingService.GetAsConcurrent("Submission");
                            presentationService.Process(statePre, false, EdgarDatasetPresentation.FILE_NAME, "Presentations");
                        }
            ));
            /*
            //Process num file
            EdgarTaskState stateNum = new EdgarTaskState(ds, repo);
            states.Add(stateNum);
            numService.Submissions = subs;
            numService.Tags = tags;
            numService.Dimensions = dimensionService.GetAsConcurrent();
            tasks.Add(Task.Factory.StartNew(() => numService.Process(stateNum, true,EdgarDatasetNumber.FILE_NAME,"Numbers")));
            */
            Task.WaitAll(tasks.ToArray());
            return states.ToArray();
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
