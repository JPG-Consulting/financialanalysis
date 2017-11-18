using Analyst.DBAccess.Contexts;
using Analyst.Domain.Edgar;
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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IAnalystRepository repository;
        private IEdgarDatasetSubmissionsService submissionService;
        private IEdgarDatasetTagService tagService;
        private IEdgarDatasetNumService numService;
        private IEdgarDatasetDimensionService dimensionService;
        private IEdgarDatasetRenderingService renderingService;
        private IEdgarDatasetPresentationService presentationService;
        private IEdgarDatasetCalculationService calcService;
        private IEdgarDatasetTextService textService;
        public EdgarDatasetService(IAnalystRepository repository, IEdgarDatasetSubmissionsService submissionService, IEdgarDatasetTagService tagService, IEdgarDatasetNumService numService, IEdgarDatasetDimensionService dimensionService, IEdgarDatasetRenderingService renderingService,IEdgarDatasetPresentationService presentationService,IEdgarDatasetCalculationService calcService, IEdgarDatasetTextService textService)
        {
            this.repository = repository;
            this.submissionService = submissionService;
            this.tagService = tagService;
            this.numService = numService;
            this.dimensionService = dimensionService;
            this.renderingService = renderingService;
            this.presentationService = presentationService;
            this.calcService = calcService;
            this.textService = textService;
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
            //https://stackify.com/log4net-guide-dotnet-logging/
            log.Info("Dataset " + id.ToString() + " requested");

            if (datasetsInProcess.ContainsKey(id))
            {
                Task t = datasetsInProcess[id];
                //TODO: how to control that task finished ok and there is no need to rerun?
                if (t.Status != TaskStatus.Running)
                {
                    t.Dispose();
                    datasetsInProcess[id] = null;
                    Run(id);
                }
            }
            else
                Run(id);
        }

        private void Run(int id)
        {
            //https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/task-based-asynchronous-programming?view=netframework-4.5.2
            Task t = new Task(() =>
            {
                EdgarDataset ds = repository.GetDataset(id);
                EdgarTaskState[] states = LoadCoreData(ds, repository);
                ManageErrors(states);
                ConcurrentDictionary<string, EdgarDatasetTag> tags = tagService.GetAsConcurrent();
                ConcurrentDictionary<string, EdgarDatasetSubmission> subs = submissionService.GetAsConcurrent();
                ConcurrentDictionary<string,EdgarDatasetDimension> dims = dimensionService.GetAsConcurrent();
                states = LoadData(ds, repository,subs,tags,dims);
                ManageErrors(states);
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
            tasks.Add(Task.Factory.StartNew(() => dimensionService.Process(stateDim,true,EdgarDatasetDimension.FILE_NAME,"Dimensions")
            ));
            Task.WaitAll(tasks.ToArray());
            return states.ToArray();
        }

        private EdgarTaskState[] LoadData(EdgarDataset ds,IAnalystRepository repo,ConcurrentDictionary<string,EdgarDatasetSubmission> subs,ConcurrentDictionary<string,EdgarDatasetTag> tags,ConcurrentDictionary<string,EdgarDatasetDimension> dims)
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

            //process calc file
            EdgarTaskState stateCalc = new EdgarTaskState(ds, repo);
            states.Add(stateCalc);
            calcService.Submissions = subs;
            calcService.Tags = tags;
            tasks.Add(Task.Factory.StartNew(() => calcService.Process(stateCalc, true, EdgarDatasetCalculation.FILE_NAME, "Calculations")));

            //process text file
            EdgarTaskState stateText = new EdgarTaskState(ds, repo);
            states.Add(stateText);
            textService.Dimensions = dims;
            textService.Submissions = subs;
            textService.Tags = tags;
            tasks.Add(Task.Factory.StartNew(() => textService.Process(stateText, true, EdgarDatasetText.FILE_NAME, "Texts")));

            //Process num file
            EdgarTaskState stateNum = new EdgarTaskState(ds, repo);
            states.Add(stateNum);
            numService.Submissions = subs;
            numService.Tags = tags;
            numService.Dimensions = dims;
            tasks.Add(Task.Factory.StartNew(() => numService.Process(stateNum, true,EdgarDatasetNumber.FILE_NAME,"Numbers")));

            Task.WaitAll(tasks.ToArray());
            return states.ToArray();
        }


        private void ManageErrors(EdgarTaskState[] states)
        {
            EdgarException edex = null;
            for (int i = 0; i < states.Length; i++)
            {
                if (states[i].ResultOk.HasValue && !states[i].ResultOk.Value)
                {
                    if (edex == null)
                        edex = new EdgarException();
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
