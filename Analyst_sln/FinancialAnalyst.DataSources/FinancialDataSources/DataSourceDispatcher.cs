using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Assets.Options;
using FinancialAnalyst.Common.Entities.Accounting;
using FinancialAnalyst.Common.Entities.Prices;
using FinancialAnalyst.Common.Interfaces;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.DataSources;
using FinancialAnalyst.DataSources.Reuters;
using System;
using System.Collections.Generic;
using System.Text;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces;
using FinancialAnalyst.Common.Entities.Markets;

namespace FinancialAnalyst.DataSources
{
    public enum DataSource
    {
        DataHubIO=1,
        EdgarSEC,
        FinancialModelingPrep,
        Google,
        Nasdaq,
        Reuters,
        USTreasury,
        Yahoo,
    }


    public class DataSourceDispatcher : IDataSource
    {
        private IStockDataDataSource assetDataDataSource;
        private IPricesDataSource pricesDataSouce;
        private IOptionChainDataSource optionChainDataSource;
        private IFinancialDataSource financialDataSource;
        private IRiskFreeRatesDataSource riskFreeRatesDataSource;
        private IAssetTypeDataSource assetTypeDataSource;
        private IStatisticsDataSource statisticsDataSource;
        private IIndexesDataSource indexesDataSource;
        private ICacheManager cacheManager;

        public DataSourceDispatcher(
            IStockDataDataSource assetDataDataSource, IPricesDataSource pricesDataSouce, IOptionChainDataSource optionChainDataSource, 
            IFinancialDataSource financialDataSource, IRiskFreeRatesDataSource riskFreeRatesDataSource, 
            IAssetTypeDataSource assetTypeDataSource, IStatisticsDataSource statisticsDataSource, IIndexesDataSource indexesDataSource, 
            ICacheManager cacheManager)
        {
            //TODO: Implementar un data source builder para poder elegir. Por ejemplo: se puede usar Yahoo, Nasdaq y Reuters para traer los balances
            //

            this.assetDataDataSource = assetDataDataSource;
            this.pricesDataSouce = pricesDataSouce;
            this.optionChainDataSource = optionChainDataSource;
            this.financialDataSource = financialDataSource;
            this.riskFreeRatesDataSource = riskFreeRatesDataSource;
            this.assetTypeDataSource = assetTypeDataSource;
            this.statisticsDataSource = statisticsDataSource;
            this.indexesDataSource = indexesDataSource;
            this.cacheManager = cacheManager;
        }

        public bool TryGetCompleteAssetData(string ticker, Exchange? exchange, AssetClass assetClass, bool includeOptionChain, bool includeFinancialStatements, out AssetBase asset, out string errorMessage)
        {
            if(assetClass != AssetClass.Stock)
            {
                throw new NotImplementedException();
            }

            if (assetDataDataSource.TryGetStockSummary(ticker, exchange, assetClass, out Stock stock, out errorMessage) == false)
            {
                asset = stock;
                return false;
            }

            asset = stock;
            if (includeOptionChain)
            {
                if (stock.LastPrice.HasValue)
                {
                    double lastPrice = (double)stock.LastPrice.Value;
                    if (TryGetOptionsChainWithTheoricalValue(ticker, exchange, lastPrice, out OptionsChain optionsChain, out errorMessage))
                    {
                        stock.OptionsChain = optionsChain;
                        stock.Volatility = optionsChain.HistoricalVolatility;
                    }
                }
                else
                {
                    if(TryGetOptionsChain(ticker, exchange, out OptionsChain optionsChain, out errorMessage))
                        stock.OptionsChain = optionsChain;
                }
            }

            if (includeFinancialStatements)
            {
                if (TryGetFinancialData(ticker, exchange, out FinancialStatements financialData, out errorMessage) == false)
                    stock.FinancialStatements = financialData;
            }

            return true;
        }
        
        public bool TryGetStockSummary(string ticker, Exchange? exchange, AssetClass assetType, out Stock asset, out string errorMessage)
        {
            return assetDataDataSource.TryGetStockSummary(ticker, exchange, assetType, out asset, out errorMessage);
        }

        public bool TryGetHistoricalPrices(string ticker, Exchange? exchange, DateTime? from, DateTime? to, PriceInterval interval, out PriceList prices, out string errorMessage)
        {
            if(cacheManager.TryGetFromCache(ticker, exchange, from, to, interval, out prices))
            {
                errorMessage = "Obtained from cache";
                return true;
            }
            return pricesDataSouce.TryGetHistoricalPrices(ticker, exchange,from,to, interval,out prices, out errorMessage);
        }

        public bool TryGetOptionsChain(string ticker, Exchange? exchange, out OptionsChain optionChain, out string errorMessage)
        {
            return optionChainDataSource.TryGetOptionsChain(ticker, exchange, out optionChain, out errorMessage);
        }

        public bool TryGetOptionsChainWithTheoricalValue(string ticker, Exchange? exchange, double lastPrice, out OptionsChain optionsChain, out string errorMessage)
        {
            DateTime? from = DateTime.Now.AddYears(-1).AddDays(-1);
            DateTime? to = DateTime.Now;
            if (TryGetHistoricalPrices(ticker, exchange, from, to, PriceInterval.Daily, out PriceList prices, out errorMessage) == false)
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

        public bool TryGetLastPrice(string ticker, Exchange? exchange, AssetClass assetType, out HistoricalPrice lastPrice, out string message)
        {
            return pricesDataSouce.TryGetLastPrice(ticker, exchange, assetType, out lastPrice, out message);
        }

        public bool TryGetAssetType(string symbol, out AssetClass assetType)
        {
            return assetTypeDataSource.TryGetAssetType(symbol, out assetType);
        }

        public bool TryGetStatistics(string ticker, Exchange? exchange, out string message)
        {
            throw new NotImplementedException();
        }

        public bool TryGetIndexData(MarketIndex index, out Dictionary<string, decimal> tickersProportions, out string message)
        {
            throw new NotImplementedException();
        }

        public bool TryGetIndexesData(out string message)
        {
            throw new NotImplementedException();
        }
    }
}
