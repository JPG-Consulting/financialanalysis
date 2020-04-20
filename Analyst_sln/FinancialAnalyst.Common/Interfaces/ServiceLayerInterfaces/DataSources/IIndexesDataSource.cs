using FinancialAnalyst.Common.Entities.Markets;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.DataSources
{
    public interface IIndexesDataSource
    {
        bool TryGetIndexData(MarketIndex index, out Dictionary<string, decimal> tickersProportions, out string message);
    }
}
