using FinancialAnalyst.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.DataSources
{
    public interface IStatisticsDataSource
    {
        bool TryGetStatistics(string ticker, Exchange? exchange, out string message);
    }
}
