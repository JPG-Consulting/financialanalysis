using Analyst.DBAccess.Contexts;
using Analyst.Domain.Edgar;
using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Services.EdgarDatasetServices
{
    public static class LogExtensions
    {
        public static void FatalInner(this log4net.ILog logger, Exception ex)
        {
            if(ex != null)
            {
                logger.Fatal(ex.Message, ex);
                logger.FatalInner(ex.InnerException);
            }
        }

    }
    public interface IEdgarDatasetService: IDisposable
    {
        IList<EdgarDataset> GetDatasets();
        EdgarDataset GetDataset(int id);
        bool IsRunning(int id);
        void ProcessDataset(int id);
        void WriteMissingFiles(int datasetID, string table);
    }
    public class EdgarDatasetService: IEdgarDatasetService
    {

        private static ConcurrentDictionary<int, Task> datasetsInProcess = new ConcurrentDictionary<int,Task>();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly bool allowProcess;

        private IAnalystRepository repository;
        private IEdgarDatasetSubmissionsService submissionService;
        private IEdgarDatasetTagService tagService;
        private IEdgarDatasetNumService numService;
        private IEdgarDatasetDimensionService dimensionService;
        private IEdgarDatasetRenderService renderingService;
        private IEdgarDatasetPresentationService presentationService;
        private IEdgarDatasetCalculationService calcService;
        private IEdgarDatasetTextService textService;
        public EdgarDatasetService(IAnalystRepository repository, IEdgarDatasetSubmissionsService submissionService, IEdgarDatasetTagService tagService, IEdgarDatasetNumService numService, IEdgarDatasetDimensionService dimensionService, IEdgarDatasetRenderService renderingService,IEdgarDatasetPresentationService presentationService,IEdgarDatasetCalculationService calcService, IEdgarDatasetTextService textService)
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
            allowProcess = true;
        }

        public EdgarDatasetService(IAnalystRepository repository)
        {
            this.repository = repository;
            allowProcess = false;
        }

        public static IEdgarDatasetService CreateOnlyForRetrieval()
        {
            return new EdgarDatasetService(new AnalystRepository(new AnalystContext()));
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

        public bool IsRunning(int id)
        {
            if (datasetsInProcess.ContainsKey(id) && datasetsInProcess[id] != null)
                return datasetsInProcess[id].Status == TaskStatus.Running;
            else
                return false;
        }

        public void ProcessDataset(int id)
        {
            //https://stackify.com/log4net-guide-dotnet-logging/
            log.Info("Datasetid " + id.ToString() + " -- Requested");
            if (allowProcess)
            {
                if (datasetsInProcess.ContainsKey(id))
                {
                    Task t = datasetsInProcess[id];
                    //TODO: how to control that task finished ok and there is no need to rerun?
                    if (t != null)
                    {
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
                else
                    Run(id);
            }
            else
            {
                throw new ApplicationException("This service doesn't allow process, you have to use other constructor");
            }
        }

        private void Run(int id)
        {
            //https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/task-based-asynchronous-programming?view=netframework-4.5.2
            Task t = new Task(() =>
            {
                try
                {
                    Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
                    log.Info("Datasetid " + id.ToString() + " -- BEGIN dataset process");
                    EdgarDataset ds = repository.GetDataset(id);
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////
                    ////BEGIN PROCESS

                    //Load Submissions, Tags and Dimensions
                    EdgarTaskState[] states = LoadSubTagDim(ds, repository);
                    ManageErrors(states);

                    //Retrieve all tags, submissions and dimensions to fill the relationship
                    //Load Calculations, Texts and Numbers
                    log.Info("Datasetid " + id.ToString() + " -- loading all tags for LoadCalTxtNum(...)");
                    ConcurrentDictionary <string, int> tags = tagService.GetAsConcurrent(id);
                    log.Info("Datasetid " + id.ToString() + " -- loading all subs for LoadCalTxtNum(...)");
                    ConcurrentDictionary<string, int> subs = submissionService.GetAsConcurrent(id);
                    log.Info("Datasetid " + id.ToString() + " -- loading all dims for LoadCalTxtNum(...)");
                    ConcurrentDictionary<string, int> dims = dimensionService.GetAsConcurrent(id);
                    log.Info("Datasetid " + id.ToString() + " -- Starting LoadCalTxtNum(...)");
                    states = LoadCalTxtNum(ds, repository, subs, tags, dims);
                    ManageErrors(states);
                    log.Info("Datasetid " + id.ToString() + " -- realising memory for dims");
                    dims = null;
                    //Load Presentations and Renders
                    log.Info("Datasetid " + id.ToString() + " -- loading all nums for LoadRenPre(...)");
                    ConcurrentDictionary<string, int> nums = numService.GetAsConcurrent(id);
                    log.Info("Datasetid " + id.ToString() + " -- loading all txt for LoadRenPre(...)");
                    ConcurrentDictionary<string, int> txts = textService.GetAsConcurrent(id);
                    log.Info("Datasetid " + id.ToString() + " -- Starting LoadRenPre(...)");
                    states = LoadRenPre(ds, repository, subs, tags, nums, txts);
                    ManageErrors(states);
                    
                    ////END PROCESS
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////

                    watch.Stop();
                    TimeSpan ts = watch.Elapsed;
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",ts.Hours, ts.Minutes, ts.Seconds,ts.Milliseconds / 10);
                    log.Info("Datasetid " + id.ToString() + " -- END dataset process -- time: " + elapsedTime);
                    
                }
                catch(Exception ex)
                {
                    log.Fatal("Datasetid " + id.ToString() + " -- Unexpected error in EdgarDatasetService.Run(" + id.ToString() + "): " + ex.Message, ex);
                    log.FatalInner(ex.InnerException);
                }
            });
            t.Start();
            datasetsInProcess.TryAdd(id, t);
        }

        private EdgarTaskState[] LoadSubTagDim(EdgarDataset ds, IAnalystRepository repo)
        {
            List<EdgarTaskState> states = new List<EdgarTaskState>();
            EdgarTaskState stateSubs, stateTag, stateDim;
            stateSubs = new EdgarTaskState(EdgarDatasetSubmission.FILE_NAME, ds, repo);
            stateTag = new EdgarTaskState(EdgarDatasetTag.FILE_NAME, ds, repo);
            stateDim = new EdgarTaskState(EdgarDatasetDimension.FILE_NAME, ds, repo);
            states.Add(stateSubs);
            states.Add(stateTag);
            states.Add(stateDim);
            IList<Task> tasks = new List<Task>();

            log.Info("Datasetid " + ds.Id.ToString() + " -- starting  submissionService.Process(...)");
            tasks.Add(Task.Factory.StartNew(() => 
                submissionService.Process(stateSubs, false, EdgarDatasetSubmission.FILE_NAME, "Submissions")//false --> to avoid to have too many threads
            ));

            log.Info("Datasetid " + ds.Id.ToString() + " -- starting  tagService.Process(...)");
            tasks.Add(Task.Factory.StartNew(() => 
                tagService.Process(stateTag,true, EdgarDatasetTag.FILE_NAME,"Tags")
            ));

            log.Info("Datasetid " + ds.Id.ToString() + " -- starting  dimensionService.Process(...)");
            tasks.Add(Task.Factory.StartNew(() => 
                dimensionService.Process(stateDim,false, EdgarDatasetDimension.FILE_NAME,"Dimensions")//false --> to avoid to have too many threads
            ));
            
            Task.WaitAll(tasks.ToArray());
            return states.ToArray();
        }

        private EdgarTaskState[] LoadCalTxtNum(EdgarDataset ds,IAnalystRepository repo,ConcurrentDictionary<string,int> subs,ConcurrentDictionary<string,int> tags,ConcurrentDictionary<string,int> dims)
        {
            List<EdgarTaskState> states = new List<EdgarTaskState>();
            List<Task> tasks = new List<Task>();
            
            //process calc file
            EdgarTaskState stateCalc = new EdgarTaskState(EdgarDatasetCalculation.FILE_NAME, ds, repo);
            states.Add(stateCalc);
            calcService.Submissions = subs;
            calcService.Tags = tags;
            log.Info("Datasetid " + ds.Id.ToString() + " -- starting  calcService.Process(...)");
            tasks.Add(Task.Factory.StartNew(() => 
                calcService.Process(stateCalc, false, EdgarDatasetCalculation.FILE_NAME, "Calculations")) //false --> to avoid to have too many threads
            );

            //process text file
            EdgarTaskState stateText = new EdgarTaskState(EdgarDatasetText.FILE_NAME, ds, repo);
            states.Add(stateText);
            textService.Dimensions = dims;
            textService.Submissions = subs;
            textService.Tags = tags;
            log.Info("Datasetid " + ds.Id.ToString() + " -- starting  textService.Process(...)");
            tasks.Add(Task.Factory.StartNew(() => 
                textService.Process(stateText, true, EdgarDatasetText.FILE_NAME, "Texts"))
            );
            
            //Process num file
            EdgarTaskState stateNum = new EdgarTaskState(EdgarDatasetNumber.FILE_NAME, ds, repo);
            states.Add(stateNum);
            numService.Dimensions = dims;
            numService.Submissions = subs;
            numService.Tags = tags;
            log.Info("Datasetid " + ds.Id.ToString() + " -- starting  numService.Process(...)");
            tasks.Add(Task.Factory.StartNew(() => 
                numService.Process(stateNum, true,EdgarDatasetNumber.FILE_NAME,"Numbers"))
            );

            Task.WaitAll(tasks.ToArray());
            return states.ToArray();
        }

        private EdgarTaskState[] LoadRenPre(EdgarDataset ds, IAnalystRepository repo, ConcurrentDictionary<string, int> subs, ConcurrentDictionary<string, int> tags,ConcurrentDictionary<string, int> nums,ConcurrentDictionary<string, int> texts)
        {
            List<EdgarTaskState> states = new List<EdgarTaskState>();
            List<Task> tasks = new List<Task>();

            EdgarTaskState stateRen = new EdgarTaskState(EdgarDatasetRender.FILE_NAME, ds, repo);
            states.Add(stateRen);

            EdgarTaskState statePre = new EdgarTaskState(EdgarDatasetPresentation.FILE_NAME, ds, repo);
            states.Add(statePre);

            tasks.Add(Task.Factory.StartNew(() =>
            {
                renderingService.Subs = subs;
                log.Info("Datasetid " + ds.Id.ToString() + " -- starting  renderingService.Process(...)");
                renderingService.Process(stateRen, true, EdgarDatasetRender.FILE_NAME, "Renders");//Presentations has a relationship to renders
                presentationService.Subs = subs;
                presentationService.Tags = tags;
                log.Info("Datasetid " + ds.Id.ToString() + " -- loading all rens for presentationService.Process(...)");
                presentationService.Renders = renderingService.GetAsConcurrent(ds.Id);
                presentationService.Nums = nums;
                presentationService.Texts = texts;
                log.Info("Datasetid " + ds.Id.ToString() + " -- starting  presentationService.Process(...)");
                presentationService.Process(statePre, true, EdgarDatasetPresentation.FILE_NAME, "Presentations");
            }
            ));
            Task.WaitAll(tasks.ToArray());
            return states.ToArray();
        }


        /// <summary>
        /// Takes all exceptions of states and it throws them to Run(..) method
        /// </summary>
        /// <param name="states"></param>
        private void ManageErrors(EdgarTaskState[] states)
        {
            EdgarException edex = null;
            for (int i = 0; i < states.Length; i++)
            {
                if (states[i].ResultOk.HasValue && !states[i].ResultOk.Value)
                {
                    if (edex == null)
                        edex = new EdgarException();
                    if (states[i].Exception != null)
                    {
                        edex.AddInnerException(states[i].Exception);
                        log.Fatal("Datasetid " + states[i].Dataset.Id.ToString() + " -- " + states[i].ProcessName + ": " + states[i].Exception.Message, states[i].Exception);
                    }
                    else
                        log.Fatal("Datasetid " + states[i].Dataset.Id.ToString() + "-- " + states[i].ProcessName + ": It failed but theres is NonSerializedAttribute excpetion");
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


        public void WriteMissingFiles(int datasetID, string table)
        {
            EdgarDataset ds = GetDataset(datasetID);
            numService.WriteMissingLines(ds, table);
        }

        public void Dispose()
        {
            //https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/task-cancellation?view=netframework-4.5.2
            //TODO: Cancel all tasks
            foreach (Task process in datasetsInProcess.Values.ToList())
            {
                
            }
            if(repository!=null)
                repository.Dispose();
            if(submissionService != null)
                submissionService.Dispose();
            if(tagService != null)
                tagService.Dispose();
            if (numService != null)
                numService.Dispose();
            if (dimensionService != null)
                dimensionService.Dispose();
            if (renderingService != null)
                renderingService.Dispose();
            if (presentationService != null)
                presentationService.Dispose();
            if (calcService != null)
                calcService.Dispose();
            if (textService != null)
                textService.Dispose();
        }
    }
}
