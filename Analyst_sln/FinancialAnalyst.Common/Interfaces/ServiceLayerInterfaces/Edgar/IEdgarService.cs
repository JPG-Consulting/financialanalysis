using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.EdgarSEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.Edgar
{
    public interface IEdgarService
    {
        IList<SECForm> GetSECForms();
        IList<SIC> GetSICs();
        IQueryable<Registrant> GetRegistrants(string sortOrder, string searchString, int pagesize, out int total);
        IQueryable GetFilings(int cik, int? year, Quarter? quarter, string sortOrder, int pagesize, out int count);
    }
}
