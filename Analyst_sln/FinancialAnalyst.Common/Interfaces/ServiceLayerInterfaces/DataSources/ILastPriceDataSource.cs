using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Prices;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.DataSources
{
    public interface ILastPriceDataSource
    {
        bool TryGetLastPrice(string ticker, Exchange? exchange, out LastPrice lastPrice, out string message);
    }
}
