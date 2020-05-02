using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Assets
{
    public enum AssetClass
    {
        Cash=1,
        Stock,
        Bond,
        ETF,
        Option,
        Fund,
        Futures,
        Unknown,
        
    }

    public enum OptionClass
    {
        Call=1, 
        Put,
        Exotic,
    }
}
