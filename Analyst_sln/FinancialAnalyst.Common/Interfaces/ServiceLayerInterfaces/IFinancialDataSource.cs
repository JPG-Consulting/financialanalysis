using FinancialAnalyst.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces
{
    public interface IFinancialDataSource
    {
        bool TryFinancialData(string ticker, Exchange? exchange, out string message);
    }
}
