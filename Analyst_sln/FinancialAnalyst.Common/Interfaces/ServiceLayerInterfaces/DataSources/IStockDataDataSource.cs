using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.DataSources
{
    public interface IStockDataDataSource
    {
        bool TryGetStockSummary(string ticker, Exchange? exchange, out Stock stock, out string errorMessage);
    }
}
