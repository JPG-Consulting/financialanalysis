using FinancialAnalyst.Common.Entities.Prices;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces
{
    public interface IRiskFreeRatesDataSource
    {
        bool TryGetRiskFreeRates(out RiskFreeRates riskFreeRate, out string message);
    }
}
