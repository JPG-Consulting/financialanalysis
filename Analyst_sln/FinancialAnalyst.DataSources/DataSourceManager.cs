using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
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
        private IAssetDataDataSource assetDataDataSource;
        private IPricesDataSource pricesDataSouce;
        private IOptionChainDataSource optionChainDataSource;
        private IFinancialDataSource financialDataSource;
        private ICacheManager cacheManager;

        public DataSourceManager(IAssetDataDataSource assetDataDataSource,IPricesDataSource pricesDataSouce,IOptionChainDataSource optionChainDataSource,IFinancialDataSource financialDataSource, ICacheManager cacheManager)
        {
            this.assetDataDataSource = assetDataDataSource;
            this.pricesDataSouce = pricesDataSouce;
            this.optionChainDataSource = optionChainDataSource;
            this.financialDataSource = financialDataSource;
            this.cacheManager = cacheManager;
        }

        public bool TryGetCompleteAssetData(string ticker, Exchange? exchange, out AssetBase asset, out string errorMessage)
        {
            if (assetDataDataSource.TryGetAssetData(ticker, exchange, out asset, out errorMessage) == false)
                return false;

            DateTime? from = null;
            DateTime? to = null;
            if (TryGetPrices(ticker, exchange, from, to, PriceInterval.Monthly, out PriceList prices, out errorMessage) == false)
                return false;

            Stock s = asset as Stock;
            if (s != null)
            {
                if (TryGetOptionsChain(ticker, exchange, out OptionChain optionChain, out errorMessage) == false)
                    return false;

                CalculateThoricalValue(s, prices, optionChain);

                return TryFinancialData(ticker, exchange, out errorMessage);
            }

            return true;
        }

        private void CalculateThoricalValue(Stock s, PriceList prices, OptionChain optionChain)
        {
            //s.Volatility = prices.CalculateVolatility();
            throw new NotImplementedException("Pending to calculate theorical price for all options");
        }

        public bool TryGetAssetData(string ticker, Exchange? exchange, out AssetBase asset, out string errorMessage)
        {
            return assetDataDataSource.TryGetAssetData(ticker, exchange, out asset, out errorMessage);
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

        public bool TryGetOptionsChain(string ticker, Exchange? exchange, out OptionChain optionChain, out string message)
        {
            return optionChainDataSource.TryGetOptionsChain(ticker, exchange, out optionChain, out message);
        }

        public bool TryFinancialData(string ticker, Exchange? exchange, out string message)
        {
            throw new NotImplementedException();
        }
    }
}
