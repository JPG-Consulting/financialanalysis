using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Assets
{
    public enum AssetType
    {
        Cash=1,
        Stock,
        Bond,
        ETF,
        Option_Call,
        Option_Put,
        Fund,
        Unknown,
        
    }
}
