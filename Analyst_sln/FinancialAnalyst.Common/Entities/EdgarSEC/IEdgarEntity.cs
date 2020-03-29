using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalyst.Common.Entities.EdgarSEC
{
    public interface IEdgarEntity
    {
        int Id { get; set; }
        string Key { get; }
    }
}
