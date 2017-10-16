using Analyst.DBAccess;
using Analyst.DBAccess.Contexts;
using Analyst.Domain;
using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Services
{
    public interface IEdgarService
    {
        List<EdgarDataset> GetDatasets();
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
                LoadInitialDatasets();
            List<EdgarDataset> datasets = Repository.GetDatasets();
            return datasets;
        }

        public void LoadInitialDatasets()
        {
            string baseurl = "/files/dera/data/financial-statement-and-notes-data-sets";
            List<EdgarDataset> datasets = new List<EdgarDataset>();
            string genericPath = baseurl + "/{0}q{1}_notes.zip";
            for (int i = 2009; i <= 2016; i++)
            {
                for (int j = 1; j <= 4; j++)
                {
                    Quarter q = (Quarter)j;

                    EdgarDataset ds = new EdgarDataset
                    {
                        RelativePath = String.Format(genericPath, i.ToString(), j),
                        Year = i,
                        Quarter = q
                    };
                    Repository.AddDataset(ds);
                }
            }
            Repository.AddDataset(new EdgarDataset { RelativePath = baseurl + "/2017q1_notes.zip", Year = 2017, Quarter = Quarter.QTR1 });
            Repository.AddDataset(new EdgarDataset { RelativePath = baseurl + "/2017q2_notes.zip", Year = 2017, Quarter = Quarter.QTR2 });
            Repository.AddDataset(new EdgarDataset { RelativePath = baseurl + "/2017q3_notes.zip", Year = 2017, Quarter = Quarter.QTR3 });
        }
    }
}
