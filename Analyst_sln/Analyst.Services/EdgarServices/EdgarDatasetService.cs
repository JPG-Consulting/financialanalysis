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
        List<EdgarDataset> GetDatasets();
        EdgarDataset ProcessDataset(int id);
        EdgarDataset GetDataset(int id);
    }
    public class EdgarDatasetService: IEdgarDatasetService
    {
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

        public List<EdgarDataset> GetDatasets()
        {
            List<EdgarDataset> datasets = repository.GetDatasets();
            return datasets;
        }

        public EdgarDataset GetDataset(int id)
        {
            EdgarDataset ds = repository.GetDataset(id);
            return ds;
        }

        public EdgarDataset ProcessDataset(int id)
        {
            //https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/task-based-asynchronous-programming?view=netframework-4.5.2

            EdgarDataset ds = repository.GetDataset(id);
            LoadNumRelatedData(ds, repository);
            LoadNums(ds, repository);
            return ds;
        }

        
        private void LoadNumRelatedData(EdgarDataset ds,IAnalystRepository repo)
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
        }

        private void LoadNums(EdgarDataset ds,IAnalystRepository repo)
        {
            EdgarTaskState state = new EdgarTaskState(ds, repo);
            ConcurrentDictionary<string, EdgarDatasetSubmissions> subs = submissionService.GetSubmissions();
            ConcurrentDictionary<string, EdgarDatasetTag> tags = tagService.GetTags();
            ConcurrentDictionary<string, EdgarDatasetDimension> dims = dimensionService.GetDimensions();

            numService.ProcessNums(state, subs, tags, dims);
        }

    }
}
