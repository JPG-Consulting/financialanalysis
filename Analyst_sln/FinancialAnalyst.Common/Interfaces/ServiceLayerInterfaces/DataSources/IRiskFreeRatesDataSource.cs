using FinancialAnalyst.Common.Entities.Prices;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.DataSources
{
    public interface IRiskFreeRatesDataSource
    {
        bool TryGetRiskFreeRates(out RiskFreeRates riskFreeRate, out string message);
    }
}
