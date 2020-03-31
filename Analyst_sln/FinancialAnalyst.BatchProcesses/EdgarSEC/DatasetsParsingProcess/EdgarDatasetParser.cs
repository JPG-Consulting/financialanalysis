using FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess.ParserStrategies.Interfaces;
using FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess.ParserStrategies.Interfaces.InterfacesForConcreteParsers;
using FinancialAnalyst.Common.Entities.EdgarSEC.Datasets;
using FinancialAnalyst.Common.Entities.EdgarSEC.Repositories;
using FinancialAnalyst.Common.Exceptions.EdgarSEC;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.Edgar;
using FinancialAnalyst.DataAccess.EdgarSEC.Repositories;
using FinancialAnalyst.DataAccess.EdgarSEC.Repositories.BulkRepositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess
{
    public static class LogExtensions
    {
        public static void FatalInner(this log4net.ILog logger, Exception ex)
        {
            if (ex != null)
            {
                logger.Fatal(ex.Message, ex);
                logger.FatalInner(ex.InnerException);
            }
        }

    }
    

    public class EdgarDatasetParser: IEdgarDatasetParser
    {
        public const int MAX_TRIALS = 5;
        private static ConcurrentDictionary<int, Task> datasetsInProcess = new ConcurrentDictionary<int, Task>();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(EdgarDatasetParser));

        private ISubmissionsDatasetParser submissionParser;
        private ITagDatasetParser tagParser;
        private INumDatasetParser numParser;
        private IDimensionDatasetParser dimensionParser;
        private IRenderDatasetParser renderingParser;
        private IPresentationDatasetParser presentationParser;
        private ICalculationDatasetParser calcParser;
        private ITextDatasetParser textParser;

        IEdgarDatasetsBulkRepository datasetsBulkRepo;

        public EdgarDatasetParser(IEdgarDatasetsBulkRepository datasetsBulkRepo)
        {
            this.datasetsBulkRepo = datasetsBulkRepo;

            if (ConfigurationManager.AppSettings["EdgarDatasetProcess"] == "bulk")
            {
                this.submissionParser = new ParserStrategies.LineByLineProcessStrategy.ConcreteParsers.SubmissionsDatasetParser(); //no need to implement BulkProcessStrategy.EdgarDatasetSubmissionsService();
                this.tagParser = new ParserStrategies.BulkProcessStrategy.ConcreteParsers.TagDatasetParser();
                this.numParser = new ParserStrategies.BulkProcessStrategy.ConcreteParsers.NumDatasetParser();
                this.dimensionParser = new ParserStrategies.BulkProcessStrategy.ConcreteParsers.DimensionDatasetParser();
                this.renderingParser = new ParserStrategies.BulkProcessStrategy.ConcreteParsers.RenderDatasetParser();
                this.presentationParser = new ParserStrategies.BulkProcessStrategy.ConcreteParsers.PresentationDatasetParser();
                this.calcParser = new ParserStrategies.BulkProcessStrategy.ConcreteParsers.CalculationDatasetParser();
                this.textParser = new ParserStrategies.BulkProcessStrategy.ConcreteParsers.TextDatasetParser();
            }
            else
            {
                this.submissionParser = new ParserStrategies.LineByLineProcessStrategy.ConcreteParsers.SubmissionsDatasetParser();
                this.tagParser = new ParserStrategies.LineByLineProcessStrategy.ConcreteParsers.TagDatasetParser();
                this.numParser = new ParserStrategies.LineByLineProcessStrategy.ConcreteParsers.NumDatasetParser();
                this.dimensionParser = new ParserStrategies.LineByLineProcessStrategy.ConcreteParsers.DimensionDatasetParser();
                this.renderingParser = new ParserStrategies.LineByLineProcessStrategy.ConcreteParsers.RenderDatasetParser();
                this.presentationParser = new ParserStrategies.LineByLineProcessStrategy.ConcreteParsers.PresentationDatasetParser();
                this.calcParser = new ParserStrategies.LineByLineProcessStrategy.ConcreteParsers.CalculationDatasetParser();
                this.textParser = new ParserStrategies.LineByLineProcessStrategy.ConcreteParsers.TextDatasetParser();
            }
        }

        public IList<EdgarDataset> GetDatasets()
        {
            using (IEdgarDatasetsRepository repository = CreateRepository())
            {
                IList<EdgarDataset> datasets = repository.Get<EdgarDataset>();
                return datasets;
            }
        }

        public EdgarDataset GetDataset(int id)
        {
            using (IEdgarDatasetsRepository repository = CreateRepository())
            {
                EdgarDataset ds = repository.GetDataset(id);
                return ds;
            }
        }

        public void DeleteDatasetFile(int id, string file)
        {
            DatasetsTables table;
            if (file == "numbers")
                table = DatasetsTables.Numbers;
            else if (file == "calculations")
                table = DatasetsTables.Calculations;
            else if (file == "texts")
                table = DatasetsTables.Texts;
            else if (file == "renders")
                table = DatasetsTables.Renders;
            else if (file == "presentations")
                table = DatasetsTables.Presentations;
            else
                throw new InvalidOperationException("Can't delete " + file);
            datasetsBulkRepo.DeleteAllRows(id, table);
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
                        ProcessDatasetTask(id);
                    }
                    log.Info("Datasetid " + id.ToString() + " -- It's in progress");
                }
                else
                    ProcessDatasetTask(id);
            }
            else
                ProcessDatasetTask(id);

        }

        private void ProcessDatasetTask(int id)
        {
            //https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/task-based-asynchronous-programming?view=netframework-4.5.2
            Task t = new Task(() =>
            {
                try
                {
                    using (IEdgarDatasetsRepository repository = CreateRepository())
                    {
                        EdgarDataset ds = repository.GetDataset(id);
                        if (ds != null)
                        {

                            //////////////////////////////////////////////////////////////////////////////////////////////////////////
                            ////BEGIN PROCESS
                            Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
                            log.Info("Datasetid " + id.ToString() + " -- BEGIN dataset process");

                            //Load Submissions, Tags and Dimensions
                            EdgarTaskState[] states = LoadSubTagDim(ds, repository);
                            bool ok = ManageErrors(states);
                            log.Info(string.Format("Datasetid {0} -- Variables after LoadSubTagDim(..): ManageErrors: {1}; Submissions: {2}/{3}; Tags: {4}/{5}; Dimensions: {6}/{7}", id, ok, ds.ProcessedSubmissions, ds.TotalSubmissions, ds.ProcessedTags, ds.TotalTags, ds.ProcessedDimensions, ds.TotalDimensions));
                            if (!ok || ds.ProcessedSubmissions != ds.TotalSubmissions || ds.ProcessedTags != ds.TotalTags || ds.ProcessedDimensions != ds.TotalDimensions)
                            {
                                log.Fatal("Process of sub, tags or dims failed, process can't continue");
                                return;
                            }
                            //Retrieve all tags, submissions and dimensions to fill the relationship
                            //Load Calculations, Texts and Numbers
                            log.Info("Datasetid " + id.ToString() + " -- loading all tags for LoadCalTxtNum(...)");
                            ConcurrentDictionary<string, int> tags = tagParser.GetAsConcurrent(id);
                            log.Info("Datasetid " + id.ToString() + " -- loading all subs for LoadCalTxtNum(...)");
                            ConcurrentDictionary<string, int> subs = submissionParser.GetAsConcurrent(id);
                            log.Info("Datasetid " + id.ToString() + " -- loading all dims for LoadCalTxtNum(...)");
                            ConcurrentDictionary<string, int> dims = dimensionParser.GetAsConcurrent(id);
                            log.Info("Datasetid " + id.ToString() + " -- Starting LoadCalTxtNum(...)");
                            states = LoadCalTxtNum(ds, repository, subs, tags, dims);
                            ok = ManageErrors(states);
                            log.Info(string.Format("Datasetid {0} -- Variables after LoadCalTxtNum(..): ManageErrors: {1}; Calculations: {2}/{3}; Texts: {4}/{5}; Numbers: {6}/{7}", id, ok, ds.ProcessedCalculations, ds.TotalCalculations, ds.ProcessedTexts, ds.TotalTexts, ds.ProcessedNumbers, ds.TotalNumbers));
                            if (!ok || ds.ProcessedCalculations != ds.TotalCalculations || ds.ProcessedTexts != ds.TotalTexts || ds.ProcessedNumbers != ds.TotalNumbers)
                            {
                                log.Fatal("Process of cal, text or nums failed, process can't continue");
                                return;
                            }
                            log.Info("Datasetid " + id.ToString() + " -- releasing dims");
                            dims.Clear();
                            dims = null;

                            //Load Presentations and Renders
                            log.Info("Datasetid " + id.ToString() + " -- loading all nums for LoadRenPre(...)");
                            ConcurrentDictionary<string, int> nums = numParser.GetAsConcurrent(id);
                            log.Info("Datasetid " + id.ToString() + " -- loading all txt for LoadRenPre(...)");
                            ConcurrentDictionary<string, int> txts = textParser.GetAsConcurrent(id);
                            log.Info("Datasetid " + id.ToString() + " -- Starting LoadRenPre(...)");
                            states = LoadRenPre(ds, repository, subs, tags, nums, txts);
                            ManageErrors(states);
                            log.Info(string.Format("Datasetid {0} -- Variables after LoadRenPre(..): ManageErrors: {1}; Reners: {2}/{3}; Presentations: {4}/{5}", id, ok, ds.ProcessedRenders, ds.TotalRenders, ds.ProcessedPresentations, ds.TotalPresentations));
                            ////END PROCESS
                            //////////////////////////////////////////////////////////////////////////////////////////////////////////
                            subs.Clear(); subs = null;
                            tags.Clear(); tags = null;
                            nums.Clear(); nums = null;
                            txts.Clear(); txts = null;
                            GC.Collect();//force GC

                            watch.Stop();
                            TimeSpan ts = watch.Elapsed;
                            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                            log.Info("Datasetid " + id.ToString() + " -- END dataset process -- time: " + elapsedTime);

                        }
                        else
                        {
                            log.Fatal("Datasetid " + id.ToString() + " does not exists");
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Fatal("Datasetid " + id.ToString() + " -- Unexpected error in EdgarDatasetService.Run(" + id.ToString() + "): " + ex.Message, ex);
                    log.FatalInner(ex.InnerException);
                }
            });
            t.Start();
            datasetsInProcess.TryAdd(id, t);
        }

        private EdgarTaskState[] LoadSubTagDim(EdgarDataset ds, IEdgarDatasetsRepository repo)
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
                submissionParser.Process(stateSubs, false, EdgarDatasetSubmission.FILE_NAME, "Submissions")//false --> to avoid to have too many threads
            ));

            log.Info("Datasetid " + ds.Id.ToString() + " -- starting  tagService.Process(...)");
            tasks.Add(Task.Factory.StartNew(() =>
            {
                if (ConfigurationManager.AppSettings["run_tag_in_parallel"] == "true")
                    tagParser.Process(stateTag, true, EdgarDatasetTag.FILE_NAME, "Tags");
                else
                    tagParser.Process(stateTag, false, EdgarDatasetTag.FILE_NAME, "Tags");

            }));

            log.Info("Datasetid " + ds.Id.ToString() + " -- starting  dimensionService.Process(...)");
            tasks.Add(Task.Factory.StartNew(() =>
            {
                dimensionParser.Process(stateDim, false, EdgarDatasetDimension.FILE_NAME, "Dimensions");
            }));

            Task.WaitAll(tasks.ToArray());
            return states.ToArray();
        }

        private EdgarTaskState[] LoadCalTxtNum(EdgarDataset ds, IEdgarDatasetsRepository repo, ConcurrentDictionary<string, int> subs, ConcurrentDictionary<string, int> tags, ConcurrentDictionary<string, int> dims)
        {
            List<EdgarTaskState> states = new List<EdgarTaskState>();
            List<Task> tasks = new List<Task>();

            //process calc file
            EdgarTaskState stateCalc = new EdgarTaskState(EdgarDatasetCalculation.FILE_NAME, ds, repo);
            states.Add(stateCalc);
            calcParser.Submissions = subs;
            calcParser.Tags = tags;
            log.Info("Datasetid " + ds.Id.ToString() + " -- starting  calcService.Process(...)");
            tasks.Add(Task.Factory.StartNew(() =>
                calcParser.Process(stateCalc, false, EdgarDatasetCalculation.FILE_NAME, "Calculations")) //false --> to avoid to have too many threads
            );

            //process text file
            EdgarTaskState stateText = new EdgarTaskState(EdgarDatasetText.FILE_NAME, ds, repo);
            states.Add(stateText);
            textParser.Dimensions = dims;
            textParser.Submissions = subs;
            textParser.Tags = tags;
            log.Info("Datasetid " + ds.Id.ToString() + " -- starting  textService.Process(...)");
            tasks.Add(Task.Factory.StartNew(() =>
            {
                textParser.Process(stateText, true, EdgarDatasetText.FILE_NAME, "Texts");
                for (int i = 0; i < MAX_TRIALS; i++)
                {
                    if (!string.IsNullOrEmpty(stateText.FileNameToReprocess))
                    {
                        string filee = stateText.FileNameToReprocess.Split('\\').Last();
                        textParser.Process(stateText, true, stateText.FileNameToReprocess, "Texts");
                    }
                }
            }));

            //Process num file
            EdgarTaskState stateNum = new EdgarTaskState(EdgarDatasetNumber.FILE_NAME, ds, repo);
            states.Add(stateNum);
            numParser.Dimensions = dims;
            numParser.Submissions = subs;
            numParser.Tags = tags;
            log.Info("Datasetid " + ds.Id.ToString() + " -- starting  numService.Process(...)");
            tasks.Add(Task.Factory.StartNew(() =>
            {
                bool parallel = ConfigurationManager.AppSettings["run_num_in_parallel"] == "true";
                numParser.Process(stateNum, parallel, EdgarDatasetNumber.FILE_NAME, "Numbers");
            }));

            Task.WaitAll(tasks.ToArray());
            return states.ToArray();
        }

        private EdgarTaskState[] LoadRenPre(EdgarDataset ds, IEdgarDatasetsRepository repo, ConcurrentDictionary<string, int> subs, ConcurrentDictionary<string, int> tags, ConcurrentDictionary<string, int> nums, ConcurrentDictionary<string, int> texts)
        {
            List<EdgarTaskState> states = new List<EdgarTaskState>();
            List<Task> tasks = new List<Task>();

            EdgarTaskState stateRen = new EdgarTaskState(EdgarDatasetRender.FILE_NAME, ds, repo);
            states.Add(stateRen);

            EdgarTaskState statePre = new EdgarTaskState(EdgarDatasetPresentation.FILE_NAME, ds, repo);
            states.Add(statePre);

            tasks.Add(Task.Factory.StartNew(() =>
            {
                renderingParser.Subs = subs;
                log.Info("Datasetid " + ds.Id.ToString() + " -- starting  renderingService.Process(...)");
                //Presentations has a relationship to renders
                renderingParser.Process(stateRen, true, EdgarDatasetRender.FILE_NAME, "Renders");
                presentationParser.Subs = subs;
                presentationParser.Tags = tags;
                log.Info("Datasetid " + ds.Id.ToString() + " -- loading all rens for presentationService.Process(...)");
                presentationParser.Renders = renderingParser.GetAsConcurrent(ds.Id);
                presentationParser.Nums = nums;
                presentationParser.Texts = texts;
                log.Info("Datasetid " + ds.Id.ToString() + " -- starting  presentationService.Process(...)");
                if (ConfigurationManager.AppSettings["run_pre_in_parallel"] == "true")
                    presentationParser.Process(statePre, true, EdgarDatasetPresentation.FILE_NAME, "Presentations"); //parallel execution
                else
                    presentationParser.Process(statePre, false, EdgarDatasetPresentation.FILE_NAME, "Presentations");//sequential execution
            }
            ));
            Task.WaitAll(tasks.ToArray());
            return states.ToArray();
        }

        private IEdgarDatasetsRepository CreateRepository()
        {
            return new EdgarRepository();
        }

        /// <summary>
        /// Takes all exceptions of states and it throws them to Run(..) method
        /// Return false is process can't continue
        /// </summary>
        /// <param name="states"></param>
        private bool ManageErrors(EdgarTaskState[] states)
        {
            bool ok = true;
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
                    {
                        log.Fatal("Datasetid " + states[i].Dataset.Id.ToString() + "-- " + states[i].ProcessName + ": It failed but theres is NonSerializedAttribute excpetion");
                    }
                    ok = false;
                }
            }
            if (edex != null)
            {
                throw edex;
            }
            return ok;
        }

        #region IDisposable implementation
        private bool isDisposed;
        private IntPtr nativeResource = IntPtr.Zero;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // The bulk of the clean-up code is implemented in Dispose(bool)

            if (isDisposed)
                return;

            if (disposing)
            {
                // free managed resources

                log.Info("Destroying EdgarDatasetService");
                //https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/task-cancellation?view=netframework-4.5.2
                //TODO: Cancel all tasks
                foreach (Task process in datasetsInProcess.Values.ToList())
                {

                }
                if (submissionParser != null)
                    submissionParser.Dispose();
                if (tagParser != null)
                    tagParser.Dispose();
                if (numParser != null)
                    numParser.Dispose();
                if (dimensionParser != null)
                    dimensionParser.Dispose();
                if (renderingParser != null)
                    renderingParser.Dispose();
                if (presentationParser != null)
                    presentationParser.Dispose();
                if (calcParser != null)
                    calcParser.Dispose();
                if (textParser != null)
                    textParser.Dispose();
            }

            // free native resources if there are any.
            if (nativeResource != IntPtr.Zero)
            {
                System.Runtime.InteropServices.Marshal.FreeHGlobal(nativeResource);
                nativeResource = IntPtr.Zero;
            }

            isDisposed = true;
        }
        #endregion
    }
}
