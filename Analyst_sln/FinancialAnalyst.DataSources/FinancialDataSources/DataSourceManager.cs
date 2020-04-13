using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Assets.Options;
using FinancialAnalyst.Common.Entities.Accounting;
using FinancialAnalyst.Common.Entities.Prices;
using FinancialAnalyst.Common.Interfaces;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.DataSources;
using FinancialAnalyst.DataSources.Reuters;
using FinancialAnalyst.DataSources.Yahoo;
using System;
using System.Collections.Generic;
using System.Text;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces;

namespace FinancialAnalyst.DataSources
{
    public class DataSourceManager : IDataSource
    {
        private IStockDataDataSource assetDataDataSource;
        private IPricesDataSource pricesDataSouce;
        private IOptionChainDataSource optionChainDataSource;
        private IFinancialDataSource financialDataSource;
        private IRiskFreeRatesDataSource riskFreeRatesDataSource;
        private ILastPriceDataSource lastPriceDataSource;
        private ICacheManager cacheManager;

        public DataSourceManager(IStockDataDataSource assetDataDataSource, IPricesDataSource pricesDataSouce, IOptionChainDataSource optionChainDataSource, IFinancialDataSource financialDataSource, IRiskFreeRatesDataSource riskFreeRatesDataSource, ILastPriceDataSource lastPriceDataSource, ICacheManager cacheManager)
        {
            this.assetDataDataSource = assetDataDataSource;
            this.pricesDataSouce = pricesDataSouce;
            this.optionChainDataSource = optionChainDataSource;
            this.financialDataSource = financialDataSource;
            this.riskFreeRatesDataSource = riskFreeRatesDataSource;
            this.lastPriceDataSource = lastPriceDataSource;
            this.cacheManager = cacheManager;
        }

        public bool TryGetCompleteStockData(string ticker, Exchange? exchange, bool includeOptionChain, bool includeFinancialStatements, out Stock stock, out string errorMessage)
        {
            if (assetDataDataSource.TryGetStockSummary(ticker, exchange, out stock, out errorMessage) == false)
                return false;

            if (includeOptionChain && stock.Price_Last.HasValue)
            {
                if (TryGetOptionsChainWithTheoricalValue(ticker, exchange, stock.Price_Last.Value, out OptionsChain optionsChain, out errorMessage) == false)
                    return false;
                stock.OptionsChain = optionsChain;
                stock.Volatility = optionsChain.HistoricalVolatility;
            }

            if (includeFinancialStatements)
            {
                if (TryGetFinancialData(ticker, exchange, out FinancialStatements financialData, out errorMessage) == false)
                    return false;
                stock.FinancialStatements = financialData;
            }

            return true;
        }
        
        public bool TryGetStockSummary(string ticker, Exchange? exchange, out Stock asset, out string errorMessage)
        {
            return assetDataDataSource.TryGetStockSummary(ticker, exchange, out asset, out errorMessage);
        }

        public bool TryGetPrices(string ticker, Exchange? exchange, DateTime? from, DateTime? to, PriceInterval interval, out PriceList prices, out string errorMessage)
        {
            if(cacheManager.TryGetFromCache(ticker, exchange, from, to, interval, out prices))
            {
                errorMessage = "Obtained from cache";
                return true;
            }
            return pricesDataSouce.TryGetPrices(ticker, exchange,from,to, interval,out prices, out errorMessage);
        }

        public bool TryGetOptionsChain(string ticker, Exchange? exchange, out OptionsChain optionChain, out string errorMessage)
        {
            return optionChainDataSource.TryGetOptionsChain(ticker, exchange, out optionChain, out errorMessage);
        }

        public bool TryGetOptionsChainWithTheoricalValue(string ticker, Exchange? exchange, double lastPrice, out OptionsChain optionsChain, out string errorMessage)
        {
            DateTime? from = DateTime.Now.AddYears(-1).AddDays(-1);
            DateTime? to = DateTime.Now;
            if (TryGetPrices(ticker, exchange, from, to, PriceInterval.Daily, out PriceList prices, out errorMessage) == false)
            {
                optionsChain = null;
                return false;
            }
            return TryGetOptionsChainWithTheoricalValue(ticker, exchange, lastPrice, prices, out optionsChain, out errorMessage);
        }

        public bool TryGetOptionsChainWithTheoricalValue(string ticker, Exchange? exchange, double lastPrice, PriceList historicalPrices, out OptionsChain optionsChain, out string errorMessage)
        {
            if (optionChainDataSource.TryGetOptionsChain(ticker, exchange, out optionsChain, out errorMessage) == false)
                return false;

            //TODO: get this data 1 per day and store it in a cache
            if (TryGetRiskFreeRates(out RiskFreeRates riskFreeRate, out errorMessage) == false)
                return false;

            OptionsCalculator.CalculateTheoricalValue(historicalPrices, optionsChain, riskFreeRate, lastPrice);
            
            return true;
        }

        public bool TryGetFinancialData(string ticker, Exchange? exchange, out FinancialStatements financialData, out string message)
        {
            /*
             * Sources
             * *******
             * 
             * Nasdaq - 4 years, from 2016 to 2019
             * https://api.nasdaq.com/api/company/AAPL/financials?frequency=1
             * 
             * Reuters - 6 years, from 2014 to 2019
             * https://www.reuters.com/companies/api/getFetchCompanyFinancials/AAPL.OQ
             * 
             * Financial statement from Apple site
             * https://investor.apple.com/sec-filings/sec-filings-details/default.aspx?FilingId=13709514
             * http://d18rn0p25nwr6d.cloudfront.net/CIK-0000320193/c0dc1bce-6ba9-4131-af22-54dab3277c8e.html#
             * 
             * The problem is that neither of both are usefull to calculate future cashflows.
             * I have to use EDGAR, but:
             * Pending to parse, store and read the datasets
             * Pending to parse, store and read the file indexes
             * Pending to access the files
             */

            return financialDataSource.TryGetFinancialData(ticker, exchange, out financialData, out message);
        }

        public bool TryGetRiskFreeRates(out RiskFreeRates riskFreeRate, out string message)
        {
            return riskFreeRatesDataSource.TryGetRiskFreeRates(out riskFreeRate,out message);
        }

        public bool TryGetFinancialData(string ticker, string cik, out string message)
        {
            throw new NotImplementedException();
        }

        public bool TryGetLastPrice(string ticker, Exchange? exchange, out LastPrice lastPrice, out string message)
        {
            return lastPriceDataSource.TryGetLastPrice(ticker, exchange, out lastPrice, out message);
        }
    }
}
