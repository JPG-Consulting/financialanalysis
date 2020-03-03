using FinancialAnalyst.Common.Entities.Assets;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Portfolios
{
    [Serializable]
    public class Portfolio
    {
        public List<AssetAllocation> AssetAllocations { get; set; }
        public decimal TotalCash { get; set; }
    }
}
