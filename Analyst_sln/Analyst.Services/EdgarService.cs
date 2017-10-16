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

namespace Analyst.Services
{
    public interface IEdgarService
    {
        List<EdgarDataset> GetDatasets();
        List<SECForm> GetSECForms();
    }

    public class EdgarService: IEdgarService
    {


        private IAnalystRepository _repo;
        //TODO: Usar Unity para Dependency Injection
        public IAnalystRepository Repository
        {
            get
            {
                if(_repo == null)
                    _repo = new AnalystRepository();
                return _repo;
            }
            set { }
        }

        public List<EdgarDataset> GetDatasets()
        {
            int dscount = Repository.GetDatasetsCount();
            if (dscount == 0)
            {
                InitialLoader.LoadSECForms(Repository);
                InitialLoader.LoadInitialDatasets(Repository);
            }
            List<EdgarDataset> datasets = Repository.GetDatasets();
            return datasets;
        }

        public List<SECForm> GetSECForms()
        {
            int formsCount = Repository.GetSECFormsCount();
            if(formsCount == 0)
            {
                InitialLoader.LoadSECForms(Repository);
            }
            List<SECForm> forms = Repository.GetSECForms();
            return forms;
        }

    }
}
