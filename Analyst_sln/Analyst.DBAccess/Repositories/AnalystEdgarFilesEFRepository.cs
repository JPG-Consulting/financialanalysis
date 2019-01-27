using System;
using System.Collections.Concurrent;
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
        void Add(MasterIndex index);
        IList<MasterIndex> GetFullIndexes();
        MasterIndex GetFullIndex(ushort year, Quarter q);
        SECForm GetSECForm(string code);
        Company GetRegistrant(int cik, string name);
        void Update(MasterIndex index, string property);
        long GetIndexEntriesCount(MasterIndex index);
    }

    public class AnalystEdgarFilesEFRepository : IAnalystEdgarFilesRepository
    {
        public const int DEFAULT_CONN_TIMEOUT = 180;
        private EdgarFilesContext Context;

        private ConcurrentDictionary<string, SECForm> secFormsCache = new ConcurrentDictionary<string, SECForm>();
        private ConcurrentDictionary<int, Company> registrantsCache = new ConcurrentDictionary<int, Company>();

        public bool ContextConfigurationAutoDetectChangesEnabled
        {
            get { return Context.Configuration.AutoDetectChangesEnabled; }
            set { Context.Configuration.AutoDetectChangesEnabled = value; }
        }

        public AnalystEdgarFilesEFRepository() : this(new EdgarFilesContext())
        {
        }

        internal AnalystEdgarFilesEFRepository(EdgarFilesContext context)
        {
            this.Context = context;
            int timeout;
            if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["ef_conn_timeout"]))
                timeout = DEFAULT_CONN_TIMEOUT;
            else
                timeout = Convert.ToInt32(ConfigurationManager.AppSettings["ef_conn_timeout"]);
            this.Context.Database.CommandTimeout = timeout;
        }

        public void Dispose()
        {
            if (this.Context != null)
                this.Context.Dispose();
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

        public void Add(MasterIndex index)
        {
            Context.MasterIndexes.Add(index);
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

        public long GetIndexEntriesCount(MasterIndex index)
        {
            return Context.IndexEntries.Where(entry => entry.MasterIndexId == index.Id).Count();
        }

        public SECForm GetSECForm(string code)
        {
            if (secFormsCache.ContainsKey(code))
                return secFormsCache[code];

            SECForm form = Context.SECForms.Where(x => x.Code == code).SingleOrDefault();
            if (form == null)
            {
                form = new SECForm() { Code = code };
                Context.SECForms.Add(form);
                Context.SaveChanges();
            }
            secFormsCache.TryAdd(code, form);
            return form;
        }

        public Company GetRegistrant(int cik,string name)
        {
            if (registrantsCache.ContainsKey(cik))
                return registrantsCache[cik];

            Company c = (Company)Context.Registrants.Where(x => x.CIK == cik).SingleOrDefault();
            if(c==null)
            {
                c = new Company() { CIK = cik, Name = name };
                Context.Registrants.Add(c);
                Context.SaveChanges();
            }
            registrantsCache.TryAdd(cik, c);
            return c;
        }

        public IList<MasterIndex> GetFullIndexes()
        {
            return Context.MasterIndexes.ToList();
        }

        public MasterIndex GetFullIndex(ushort year, Quarter quarter)
        {
            return Context.MasterIndexes.Where(index => index.Year == year && index.Quarter == quarter).SingleOrDefault();
        }

        

        public void Update(MasterIndex index, string property)
        {
            Context.Entry<MasterIndex>(index).Property(property).IsModified = true;
            Context.SaveChanges();

        }
    }
}
