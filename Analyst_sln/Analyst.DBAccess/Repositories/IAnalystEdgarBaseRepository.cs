using Analyst.Domain.Edgar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.DBAccess.Repositories
{
    public interface IAnalystEdgarBaseRepository
    {
        void Add(SECForm sECForm);
        void Add(SIC sic);
        int GetSECFormsCount();
        int GetSICCount();
    }
}
