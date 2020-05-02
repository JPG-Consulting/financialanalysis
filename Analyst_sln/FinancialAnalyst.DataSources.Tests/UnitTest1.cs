using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Interfaces;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.DataSources;
using FinancialAnalyst.DataSources.FinancialDataSources.Nasdaq;
using FinancialAnalyst.DataSources.FinancialDataSources.Yahoo;
using FinancialAnalyst.DataSources.Reuters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace FinancialAnalyst.DataSources.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_Serialization()
        {
            IStockDataDataSource stockDataDataSource = new ReutersDataSource();
            IPricesDataSource pricesDataSource  = new YahooDataSource();
            IOptionChainDataSource optionChainDataSource = new NasdaqDataSource();
            IFinancialDataSource financialDataSource = new NasdaqDataSource();
            IRiskFreeRatesDataSource riskFreeRatesDataSource = null;
            IAssetTypeDataSource assetTypeDataSource = null;
            IStatisticsDataSource statisticsDataSource = null;
            IIndexesDataSource indexesDataSource = null;
            ICacheManager cacheManager = new FileCacheManager();

            IDataSource dataSource = new DataSourceDispatcher(stockDataDataSource, pricesDataSource, optionChainDataSource, financialDataSource, riskFreeRatesDataSource, assetTypeDataSource, statisticsDataSource, indexesDataSource, cacheManager);
            bool ok = dataSource.TryGetCompleteAssetData("AAPL", Common.Entities.Exchange.NASDAQ,AssetClass.Stock, true, true, out AssetBase asset, out string message);
            Assert.IsTrue(ok);
            string json = JsonConvert.SerializeObject(asset);
            AssetBase a = JsonConvert.DeserializeObject<AssetBase>(json);
        }
    }
}
