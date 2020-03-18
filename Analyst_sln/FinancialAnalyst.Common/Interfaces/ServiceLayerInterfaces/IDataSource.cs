using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Prices;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces
{
    public interface IDataSource
    {
        bool TryGetAssetData(string ticker, Exchange? exchange, out AssetBase asset,out string errorMessage);
        bool TryGetPrices(string ticker, Exchange? exchange, DateTime? from, DateTime? to, PriceInterval interval, out PriceList prices,  out string errorMessage);
        bool TryGetOptionsChain(string ticker, Exchange? exchange, out string message);
    }
}
