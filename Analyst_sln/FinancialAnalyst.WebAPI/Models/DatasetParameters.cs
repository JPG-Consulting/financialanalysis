using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAnalyst.WebAPI.Models
{
    [Serializable]
    public class DatasetParameters
    {
        public int id { get; set; }
#pragma warning disable CA2235 // Mark all non-serializable fields
        public string file { get; set; }
#pragma warning restore CA2235 // Mark all non-serializable fields
    }
}
