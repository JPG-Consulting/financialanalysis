using Analyst.DBAccess;
using Analyst.DBAccess.Contexts;
using Analyst.Domain;
using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyst.Domain.Edgar;
using System.Configuration;
using System.IO;
using System.Threading;

namespace Analyst.Services
{
    public interface IEdgarService
    {
        List<EdgarDataset> GetDatasets();
        List<SECForm> GetSECForms();
        List<SIC> GetSICs();
        EdgarDataset ProcessDataset(int id);
        EdgarDataset GetDataset(int id);
    }

    public class EdgarService : IEdgarService
    {
        private IAnalystRepository repository;
        private ISubmissionService submissionService;
        private ITagService tagService;

        public EdgarService(IAnalystRepository repository, ISubmissionService submissionService, ITagService tagService)
        {
            this.repository = repository;
            this.submissionService = submissionService;
            this.tagService = tagService;
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

        public List<SECForm> GetSECForms()
        {
            List<SECForm> forms = repository.GetSECForms();
            return forms;
        }

        public List<SIC> GetSICs()
        {
            List<SIC> sics = repository.GetSICs();
            return sics;
        }


        public EdgarDataset ProcessDataset(int id)
        {
            //https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/task-based-asynchronous-programming?view=netframework-4.5.2

            EdgarDataset ds = repository.GetDataset(id);
            int taskAmount = 2;
            EdgarTaskState[] states = new EdgarTaskState[taskAmount];
            for (int i = 0; i < states.Count(); i++)
                states[i] = new EdgarTaskState(ds);
            Task[] taskArray = new Task[taskAmount];
            //taskArray[0] = Task.Factory.StartNew(() => submissionService.ProcessSubmissions(states[0]));
            taskArray[1] = Task.Factory.StartNew(() => tagService.ProcessTags(states[1]));

            //Task.WaitAll(taskArray);
            return ds;
        }
    }

    public class EdgarTaskState
    {
        public bool? Result { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }

        private EdgarDataset ds;
        public EdgarDataset Dataset { get { return ds; } }
        public EdgarTaskState(EdgarDataset ds)
        {
            this.ds = ds;
        }

    }
   
}
