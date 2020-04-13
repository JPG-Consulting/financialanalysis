using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Prices;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.DataSources
{
    public interface IDataSource: IPricesDataSource, IStockDataDataSource, IOptionChainDataSource, IFillingsDataSource,IFinancialDataSource, ILastPriceDataSource
    {
        bool TryGetCompleteStockData(string ticker, Exchange? exchange, bool includeOptionChain, bool includeFinancialStatements, out Stock stock, out string errorMessage);
    }
}
