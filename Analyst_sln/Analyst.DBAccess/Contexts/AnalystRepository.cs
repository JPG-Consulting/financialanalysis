using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyst.Domain.Edgar;
using Analyst.Domain.Edgar.Datasets;

namespace Analyst.DBAccess.Contexts
{
    public interface IAnalystRepository
    {
        List<EdgarDataset> GetDatasets();
        void AddDataset(EdgarDataset ds);
        int GetDatasetsCount();
        void AddSECForm(SECForm sECForm);
        int GetSECFormsCount();
        List<SECForm> GetSECForms();
    }

    public class AnalystRepository: IAnalystRepository
    {

        private AnalystContext _context;
        internal AnalystContext Context
        {
            //TODO: Implementar inyeccion de dependencias
            get {
                if(_context == null)
                    _context = new AnalystContext();
                return _context;
            }
            set { }
        }
        public List<EdgarDataset> GetDatasets()
        {
            return Context.DataSets.ToList();
        }

        public int GetDatasetsCount()
        {
            return Context.DataSets.Count();
        }

        public void AddDataset(EdgarDataset ds)
        {
            Context.DataSets.Add(ds);
            Context.SaveChanges();
        }


        public int GetSECFormsCount()
        {
            return Context.SECForms.Count();
        }

        public void AddSECForm(SECForm sf)
        {
            Context.SECForms.Add(sf);
            Context.SaveChanges();
        }

        public List<SECForm> GetSECForms()
        {
            return Context.SECForms.ToList();
        }
    }
}
