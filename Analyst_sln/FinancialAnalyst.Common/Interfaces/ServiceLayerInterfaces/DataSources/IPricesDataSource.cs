using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Prices;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.DataSources
{
    public interface IPricesDataSource
    {
        bool TryGetHistoricalPrices(string ticker, Exchange? exchange, DateTime? from, DateTime? to, PriceInterval interval, out PriceList prices, out string errorMessage);
        bool TryGetLastPrice(string ticker, Exchange? exchange, AssetClass assetType, out HistoricalPrice lastPrice, out string message);
    }
}
