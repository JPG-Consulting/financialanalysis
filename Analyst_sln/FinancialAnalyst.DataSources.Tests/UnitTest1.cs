using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Interfaces;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.DataSources;
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
            IStockDataDataSource stockDataDataSource = new ReutersDataSource();
            IPricesDataSource pricesDataSource  = new YahooDataSource();
            IOptionChainDataSource optionChainDataSource = new NasdaqDataSource();
            IFinancialDataSource financialDataSource = new NasdaqDataSource();
            IRiskFreeRatesDataSource riskFreeRatesDataSource = null;
            ICacheManager cacheManager = new FileCacheManager();

            IDataSource dataSource = new DataSourceManager(stockDataDataSource, pricesDataSource, optionChainDataSource, financialDataSource, riskFreeRatesDataSource, cacheManager);
            bool ok = dataSource.TryGetCompleteStockData("AAPL", Common.Entities.Exchange.NASDAQ, true, true, out Stock stock, out string message);
            Assert.IsTrue(ok);
            string json = JsonConvert.SerializeObject(stock);
            Stock s = JsonConvert.DeserializeObject<Stock>(json);
        }
    }
}
