using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Accounting;
using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Prices;
using FinancialAnalyst.Common.Interfaces;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.DataSources.Edgar
{
    public class EdgarSECDataSource : IFinancialDataSource, IFillingsDataSource
    {
        public bool TryGetFinancialData(string ticker, Exchange? exchange, out FinancialStatements financialData, out string message)
        {
            throw new NotImplementedException();
        }

        public bool TryGetFinancialData(string ticker, string cik, out string message)
        {
            throw new NotImplementedException();
        }
    }
}
