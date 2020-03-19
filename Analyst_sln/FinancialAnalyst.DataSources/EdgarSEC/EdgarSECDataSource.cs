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
    public class EdgarSECDataSource : IAssetDataDataSource, IFillingsDataSource
    {
        public bool TryGetAssetData(string ticker, Exchange? exchange, out AssetBase asset, out string errorMessage)
        {
            throw new NotImplementedException();
        }
    }
}
