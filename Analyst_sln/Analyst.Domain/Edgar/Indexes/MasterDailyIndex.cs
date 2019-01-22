using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Domain.Edgar.Indexes
{
    [Serializable]
    [DataContract]
    public class MasterDailyIndex : IndexBase<int>
    {
        public DateTime IndexDate { get; }
        public override string Key { get { return Year.ToString() + Quarter.ToString() + IndexDate.ToString("yyyyMMdd"); } }
    }
}
