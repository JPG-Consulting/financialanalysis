using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Assets.Options;
using FinancialAnalyst.Common.Entities.Prices;
using FinancialAnalyst.Common.Interfaces;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces;
using FinancialAnalyst.DataSources.Reuters;
using FinancialAnalyst.DataSources.Yahoo;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.DataSources
{
    public class DataSourceManager:IDataSource
    {
        private IStockDataDataSource assetDataDataSource;
        private IPricesDataSource pricesDataSouce;
        private IOptionChainDataSource optionChainDataSource;
        private IFinancialDataSource financialDataSource;
        private ICacheManager cacheManager;

        public DataSourceManager(IStockDataDataSource assetDataDataSource,IPricesDataSource pricesDataSouce,IOptionChainDataSource optionChainDataSource,IFinancialDataSource financialDataSource, ICacheManager cacheManager)
        {
            this.assetDataDataSource = assetDataDataSource;
            this.pricesDataSouce = pricesDataSouce;
            this.optionChainDataSource = optionChainDataSource;
            this.financialDataSource = financialDataSource;
            this.cacheManager = cacheManager;
        }

        public bool TryGetCompleteStockData(string ticker, Exchange? exchange, out Stock stock, out string errorMessage)
        {
            if (assetDataDataSource.TryGetStockData(ticker, exchange, out stock, out errorMessage) == false)
                return false;

            DateTime? from = DateTime.Now.AddYears(-1).AddDays(-1);
            DateTime? to = DateTime.Now;
            if (TryGetPrices(ticker, exchange, from, to, PriceInterval.Daily, out PriceList prices, out errorMessage) == false)
                return false;

            Stock s = stock as Stock;
            if (s != null)
            {
                if (TryGetOptionsChain(ticker, exchange, out OptionsChain optionsChain, out errorMessage) == false)
                    return false;

                stock.OptionsChain = optionsChain;

                //https://www.treasury.gov/resource-center/data-chart-center/interest-rates/pages/TextView.aspx?data=yield
                RiskFreeRates riskFreeRate = new RiskFreeRates()
                {
                    TwoYears = 0.37 / 100, //r=0.37%=0.0037
                };

                OptionsCalculator.CalculateThoricalValue(s, prices, optionsChain, riskFreeRate);

                return TryFinancialData(ticker, exchange, out errorMessage);
            }

            return true;
        }

        
        public bool TryGetStockData(string ticker, Exchange? exchange, out Stock asset, out string errorMessage)
        {
            return assetDataDataSource.TryGetStockData(ticker, exchange, out asset, out errorMessage);
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

        public bool TryGetOptionsChain(string ticker, Exchange? exchange, out OptionsChain optionsChain, out string message)
        {
            return optionChainDataSource.TryGetOptionsChain(ticker, exchange, out optionsChain, out message);
        }

        public bool TryFinancialData(string ticker, Exchange? exchange, out string message)
        {
            message = "Pending to get financial data";
            return true;
        }
    }
}
