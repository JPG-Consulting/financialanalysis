using Analyst.DBAccess.Contexts;
using Analyst.Domain.Edgar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.DBAccess.Repositories
{
    public interface IAnalystEdgarRepository
    {
        IList<Registrant> GetCompanies();
    }
    public class AnalystEdgarRepository: IAnalystEdgarRepository
    {
        EdgarDatasetsContext context;
        public AnalystEdgarRepository()
        {
            context = new EdgarDatasetsContext();
        }

        public IList<Registrant> GetCompanies()
        {
            return context.Registrants.ToList<Registrant>();
        }
    }
}
