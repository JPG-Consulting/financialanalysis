using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Accounting;
using FinancialAnalyst.Common.Entities.Prices;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.DataSources;

namespace FinancialAnalyst.DataSources.FinancialModelingPrep
{
    /// <summary>
    /// https://financialmodelingprep.com/developer/docs/
    /// </summary>
    public class FinancialModelingPrepDataSource : IDataSource
    {
        public bool TryGetCompleteStockData(string ticker, Exchange? exchange, out Stock asset, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool TryGetFinancialData(string ticker, Exchange? exchange, out FinancialStatements financialData, out string message)
        {
            //https://financialmodelingprep.com/developer/docs/#Company-Financial-Statements
            throw new NotImplementedException();
        }

        public bool TryGetFinancialData(string ticker, string cik, out string message)
        {
            throw new NotImplementedException();
        }

        public bool TryGetOptionsChain(string ticker, Exchange? exchange, out OptionsChain optionChain, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool TryGetPrices(string ticker, Exchange? exchange, DateTime? from, DateTime? to, PriceInterval interval, out PriceList prices, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool TryGetStockData(string ticker, Exchange? exchange, out Stock stock, out string errorMessage)
        {
            throw new NotImplementedException();
        }
    }
}
