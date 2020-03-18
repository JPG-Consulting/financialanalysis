using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Prices;
using FinancialAnalyst.Common.Interfaces;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.DataSources.Edgar
{
    public class EdgarSECDataSource : IDataSource
    {
        public bool TryGetAssetData(string ticker, Exchange? market, out AssetBase asset, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool TryGetOptionsChain(string ticker, Exchange? exchange, out string message)
        {
            throw new NotImplementedException();
        }

        public bool TryGetPrices(string ticker, Exchange? exchange, DateTime? from, DateTime? to, PriceInterval interval, out PriceList prices, out string errorMessage)
        {
            throw new NotImplementedException();
        }
    }
}
