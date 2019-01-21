using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Domain.Edgar.Indexes
{
    public class MasterDailyIndex : IndexBase<int>
    {
        public DateTime IndexDate { get; }
    }
}
