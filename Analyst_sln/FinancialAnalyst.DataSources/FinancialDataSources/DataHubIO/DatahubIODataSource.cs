using FinancialAnalyst.Common.Entities.Markets;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.DataSources;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.DataSources.FinancialDataSources.DataHubIO
{
    public class DatahubIODataSource : IIndexesDataSource
    {
        /*
         * https://datahub.io/collections/stock-market-data
         * https://datahub.io/collections/economic-data
         * https://datahub.io/collections/property-prices
         * https://datahub.io/collections/inflation
         */
        public bool TryGetIndexData(MarketIndex index, out Dictionary<string, decimal> tickersProportions, out string message)
        {
            throw new NotImplementedException();
        }

        public bool TryGetIndexesData(out string message)
        {
            throw new NotImplementedException();
        }
    }
}
