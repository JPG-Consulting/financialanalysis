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
        List<SIC> GetSICs();
        int GetSICCount();
        void AddSIC(SIC sIC);
        EdgarDataset GetDataset(int id);
        Registrant GetRegistrant(string cik);
        SECForm GetSECForm(string code);
        void AddRegistrant(Registrant r);
        SIC GetSIC(string code);
        void Save(EdgarDataset ds, EdgarDatasetSubmissions sub);
        void Save(EdgarDataset ds, EdgarDatasetTag tag);
        EdgarDatasetTag GetTag(string tag, string version);
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
            return Context.DataSets.OrderBy(x=> x.Year).ToList();
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

        public List<SIC> GetSICs()
        {
            return Context.SICs.ToList();
        }

        public int GetSICCount()
        {
            return Context.SICs.Count();
        }

        public void AddSIC(SIC sic)
        {
            Context.SICs.Add(sic);
            Context.SaveChanges();
        }

        public EdgarDataset GetDataset(int id)
        {
            return Context.DataSets.Single(x => x.Id == id);
        }

        public Registrant GetRegistrant(string cik)
        {
            int iCik = int.Parse(cik);
            return Context.Registrants.Where(x => x.CIK == iCik).SingleOrDefault();
        }

        public SIC GetSIC(string code)
        {
            int iCode = short.Parse(code);
            return Context.SICs.Where(x => x.Code == iCode).SingleOrDefault();
        }

        public SECForm GetSECForm(string code)
        {
            SECForm form = Context.SECForms.Where(x => x.Code == code).SingleOrDefault();
            if (form == null)
            {
                form = new SECForm() { Code = code };
                Context.SECForms.Add(form);
                Context.SaveChanges();
            }
            return form;
        }

        public void AddRegistrant(Registrant r)
        {
            Context.Registrants.Add(r);
            Context.SaveChanges();
        }

        public void Save(EdgarDataset ds, EdgarDatasetSubmissions sub)
        {
            ds.Submissions.Add(sub);
            Context.Submissions.Add(sub);
            Context.SaveChanges();
        }

        public void Save(EdgarDataset ds, EdgarDatasetTag tag)
        {
            ds.Tags.Add(tag);
            Context.Tags.Add(tag);
            Context.SaveChanges();
        }
        public EdgarDatasetTag GetTag(string tag,string version)
        {
            return Context.Tags.Where(x => x.Tag == tag && x.Version==version).SingleOrDefault();
        }
    }
}
