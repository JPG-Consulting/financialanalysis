using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces
{
    public interface IStockDataDataSource
    {
        bool TryGetStockData(string ticker, Exchange? exchange, out Stock stock, out string errorMessage);
    }
}
