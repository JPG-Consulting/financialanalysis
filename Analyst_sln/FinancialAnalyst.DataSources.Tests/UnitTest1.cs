using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Interfaces;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces;
using FinancialAnalyst.DataSources.Nasdaq;
using FinancialAnalyst.DataSources.Reuters;
using FinancialAnalyst.DataSources.Yahoo;
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
            IPricesDataSource pricesDataSource  = new YahooDataSource();
            IStockDataDataSource stockDataDataSource = new ReutersDataSource();
            IOptionChainDataSource optionChainDataSource = new NasdaqDataSource();
            IFinancialDataSource financialDataSource = new NasdaqDataSource();
            ICacheManager cacheManager = new FileCacheManager();
            IDataSource dataSource = new DataSourceManager(stockDataDataSource, pricesDataSource, optionChainDataSource, financialDataSource, cacheManager);
            bool ok = dataSource.TryGetCompleteStockData("AAPL", Common.Entities.Exchange.NASDAQ, out Stock stock, out string message);
            Assert.IsTrue(ok);
            string json = JsonConvert.SerializeObject(stock);
            Stock s = JsonConvert.DeserializeObject<Stock>(json);
        }
    }
}
