using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Accounting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.DataSources
{
    public interface IFinancialDataSource
    {
        bool TryGetFinancialData(string ticker, Exchange? exchange, out FinancialStatements financialData, out string message);
    }
}
