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

    public class EdgarService: IEdgarService
    {
        private IAnalystRepository repository;
        private ISubmissionService submissionService;


        public EdgarService(IAnalystRepository repository,ISubmissionService submissionService)
        {
            this.repository = repository;
            this.submissionService = submissionService;
        }

        public List<EdgarDataset> GetDatasets()
        {
            InitialLoader.LoadInitialData(repository);
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
            InitialLoader.LoadInitialData(repository);
            List<SECForm> forms = repository.GetSECForms();
            return forms;
        }

        public List<SIC> GetSICs()
        {
            InitialLoader.LoadInitialData(repository);
            List<SIC> sics = repository.GetSICs();
            return sics;
        }


        public EdgarDataset ProcessDataset(int id)
        {
            EdgarDataset ds = repository.GetDataset(id);
            submissionService.ProcessSubmissions(ds);
            (new TagService()).ProcessTags(ds);
            return ds;
        }


        
    }
}
