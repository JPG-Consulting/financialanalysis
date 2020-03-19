using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Prices;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces
{
    public interface IDataSource: IPricesDataSource, IAssetDataDataSource, IOptionChainDataSource, IFillingsDataSource,IFinancialDataSource
    {
        bool TryGetCompleteAssetData(string ticker, Exchange? exchange, out AssetBase asset, out string errorMessage);
    }
}
