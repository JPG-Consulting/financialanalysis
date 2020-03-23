using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Prices;
using FinancialAnalyst.Common.Interfaces;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.DataSources.Google
{
    /// <summary>
    /// https://developers.google.com/sites/faq_gdata
    /// https://stackoverflow.com/questions/46070126/google-finance-json-stock-quote-stopped-working
    /// </summary>
    public class GoogleDataSource : IDataSource
    {
        public bool TryGetFinancialData(string ticker, Exchange? exchange, out string message)
        {
            throw new NotImplementedException();
        }

        public bool TryGetStockData(string ticker, Exchange? market, out Stock asset, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool TryGetCompleteStockData(string ticker, Exchange? exchange, out Stock asset, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool TryGetOptionsChain(string ticker, Exchange? exchange, out string message)
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
    }
}
