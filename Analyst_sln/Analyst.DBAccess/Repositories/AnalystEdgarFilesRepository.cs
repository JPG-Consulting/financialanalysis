using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyst.DBAccess.Contexts;
using Analyst.Domain;
using Analyst.Domain.Edgar;
using Analyst.Domain.Edgar.Indexes;

namespace Analyst.DBAccess.Repositories
{
    public interface IAnalystEdgarFilesRepository: IAnalystEdgarBaseRepository, IDisposable
    {
        void Add(MasterFullIndex index);
        IList<MasterFullIndex> GetFullIndexes();
        MasterFullIndex GetFullIndex(ushort year, Quarter q);
        void SaveIndexEntries(MasterFullIndex index, IList<IndexEntry> entries);
        SECForm GetSECForm(string code);
        Company GetRegistrant(int cik, string name);
        
    }

    public class AnalystEdgarFilesRepository : IAnalystEdgarFilesRepository
    {
        public const int DEFAULT_CONN_TIMEOUT = 180;
        private EdgarFilesContext Context;

        public bool ContextConfigurationAutoDetectChangesEnabled
        {
            get { return Context.Configuration.AutoDetectChangesEnabled; }
            set { Context.Configuration.AutoDetectChangesEnabled = value; }
        }

        public AnalystEdgarFilesRepository() : this(new EdgarFilesContext())
        {
        }

        internal AnalystEdgarFilesRepository(EdgarFilesContext context)
        {
            this.Context = context;
            int timeout;
            if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["ef_conn_timeout"]))
                timeout = DEFAULT_CONN_TIMEOUT;
            else
                timeout = Convert.ToInt32(ConfigurationManager.AppSettings["ef_conn_timeout"]);
            this.Context.Database.CommandTimeout = timeout;
        }

        public void Add(SECForm sf)
        {
            Context.SECForms.Add(sf);
            Context.SaveChanges();
        }

        public void Add(SIC sic)
        {
            Context.SICs.Add(sic);
            Context.SaveChanges();
        }

        public void Add(MasterFullIndex index)
        {
            Context.MasterFullIndexes.Add(index);
            Context.SaveChanges();
        }

        public int GetSECFormsCount()
        {
            return Context.SECForms.Count();
        }

        public int GetSICCount()
        {
            return Context.SICs.Count();
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

        public Company GetRegistrant(int cik,string name)
        {
            Company c = (Company)Context.Registrants.Where(x => x.CIK == cik).SingleOrDefault();
            if(c==null)
            {
                c = new Company() { CIK = cik, Name = name };
                Context.Registrants.Add(c);
                Context.SaveChanges();
            }
            return c;
        }

        public IList<MasterFullIndex> GetFullIndexes()
        {
            return Context.MasterFullIndexes.ToList();
        }

        public MasterFullIndex GetFullIndex(ushort year, Quarter quarter)
        {
            return Context.MasterFullIndexes.Where(index => index.Year == year && index.Quarter == quarter).SingleOrDefault();
        }

        public void SaveIndexEntries(MasterFullIndex index, IList<IndexEntry> entries)
        {
            foreach(IndexEntry entry in entries)
            {
                entry.MasterIndex = index;
                entry.MasterIndexId = index.Id;
                entry.Company = GetRegistrant(entry.CIK, entry.CompanyName);
                Context.Registrants.Attach(entry.Company);
                Context.SECForms.Attach(entry.FormType);
                Context.IndexEntries.Add(entry);
                Context.SaveChanges();
            }
            index.IsComplete = true;
            Context.SaveChanges();
        }

        public void Dispose()
        {
            if (this.Context != null)
                this.Context.Dispose();
        }

        
    }
}
