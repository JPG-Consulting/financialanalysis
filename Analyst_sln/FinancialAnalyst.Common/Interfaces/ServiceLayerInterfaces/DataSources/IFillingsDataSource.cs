using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.DataSources
{
    public interface IFillingsDataSource
    {
        bool TryGetFinancialData(string ticker, string cik, out string message);
    }
}
