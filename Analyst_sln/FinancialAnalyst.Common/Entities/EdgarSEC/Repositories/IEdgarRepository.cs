using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data;
using System.Configuration;
using System.Collections.Concurrent;
using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.EdgarSEC.Datasets;
using FinancialAnalyst.Common.Entities.EdgarSEC.Indexes;

namespace FinancialAnalyst.Common.Entities.EdgarSEC.Repositories
{
    public interface IEdgarRepository
    {
        bool ContextConfigurationAutoDetectChangesEnabled { get; set; }

        void Add(SECForm sECForm);
        void Add(SIC sic);
        IList<T> Get<T>() where T : class, IEdgarEntity;
        int GetSECFormsCount();
        int GetSICCount();
        SECForm GetSECForm(string code);
        SIC GetSIC(string code);
        Registrant GetRegistrant(string cik);
        Registrant GetRegistrant(int cik, string name);
        IQueryable<Registrant> GetRegistrants(string sortOrder, string searchString, int pagesize, out int total);
        IQueryable GetFilings(int cik, int? year, Quarter? quarter, string sortOrder, int pagesize, out int count);
        
    }

}
