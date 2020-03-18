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
    public class DataSourceManager:IDataSourceManager
    {
        private IDataSource reutersdataSource;
        private IDataSource yahooDataSource;
        private ICacheManager cacheManager;

        public DataSourceManager(ICacheManager cacheManager)
        {
            this.reutersdataSource = new ReutersDataSource();
            this.yahooDataSource = new YahooDataSource();
            this.cacheManager = cacheManager;
        }

        public bool TryGetAssetData(string ticker, Exchange? exchange, out AssetBase asset, out string errorMessage)
        {
            return reutersdataSource.TryGetAssetData(ticker, exchange, out asset, out errorMessage);
        }

        public bool TryGetOptionsChain(string ticker, Exchange? exchange, out string message)
        {
            throw new NotImplementedException();
        }

        public bool TryGetPrices(string ticker, Exchange? exchange, DateTime? from, DateTime? to, PriceInterval interval, out PriceList prices, out string errorMessage)
        {
            if(cacheManager.TryGetFromCache(ticker, exchange, from, to, interval, out prices))
            {
                errorMessage = "Obtained from cache";
                return true;
            }
            return yahooDataSource.TryGetPrices(ticker, exchange,from,to, interval,out prices, out errorMessage);
        }
    }
}
