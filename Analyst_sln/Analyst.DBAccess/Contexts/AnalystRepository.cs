using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyst.Domain.Edgar;
using Analyst.Domain.Edgar.Datasets;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace Analyst.DBAccess.Contexts
{
    public interface IAnalystRepository:IDisposable
    {
        bool ContextConfigurationAutoDetectChangesEnabled { get; set; }

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
        void Save(EdgarDatasetTag tag);
    }

    public class AnalystRepository: IAnalystRepository
    {

        private AnalystContext Context;
        
        public AnalystRepository(AnalystContext context)
        {
            this.Context = context;
        }

        public void Dispose()
        {
            if (this.Context != null)
                this.Context.Dispose();
        }

        public bool ContextConfigurationAutoDetectChangesEnabled
        {
            get { return Context.Configuration.AutoDetectChangesEnabled; }
            set { Context.Configuration.AutoDetectChangesEnabled = value; }
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
            ds.SubmissionsProcessed = ds.SubmissionsProcessed + 1;
            Context.SaveChanges();
        }

        public void Save(EdgarDataset ds, EdgarDatasetTag tag)
        {
            ds.Tags.Add(tag);
            ds.TagsProcessed = ds.TagsProcessed + 1;
            Context.SaveChanges();
        }

        public void Save(EdgarDatasetTag tag)
        {
            try
            {
                Context.Tags.Add(tag);
                Context.SaveChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public EdgarDatasetTag GetTag(string tag,string version)
        {
            /*
            string sql = "select * from [dbo].[EdgarDatasetTags] where " +
                "tag = '" + tag + "' COLLATE SQL_Latin1_General_CP1_CS_AS " +
                "AND version = '" + version + "' COLLATE SQL_Latin1_General_CP1_CS_AS ";
            return Context.Tags.SqlQuery(sql).SingleOrDefault();
            */
            DbQuery<EdgarDatasetTag> q = (DbQuery<EdgarDatasetTag>) Context.Tags.Where(t => t.Tag == tag && t.Version == version);
            return q.SingleOrDefault();

        }


    }
}
