using FinancialAnalyst.Common.Entities.Assets;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Portfolios
{
    [Serializable]
    public class AssetAllocation
    {
        public string Ticker { get; set; }
        public Market Ticker_Market { get; set; }
        public decimal Amount { get; set; }
        public decimal Percentage { get; set; }
    }
}
