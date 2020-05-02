using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Prices;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.DataSources
{
    public interface IDataSource: IPricesDataSource, IStockDataDataSource, IOptionChainDataSource, IFillingsDataSource, IFinancialDataSource, IAssetTypeDataSource, IStatisticsDataSource, IIndexesDataSource
    {
        bool TryGetCompleteAssetData(string ticker, Exchange? exchange, AssetClass assetClass, bool includeOptionChain, bool includeFinancialStatements, out AssetBase asset, out string errorMessage);
    }
}
