using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Portfolios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinancialAnalyst.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfoliosController : ControllerBase
    {
        [HttpGet("getportfoliosbyuser")]
        public IEnumerable<Portfolio> GetPortfoliosByUser(string userId)
        {
#if DEBUG
            List<Portfolio> portfolios = new List<Portfolio>();
            Portfolio portfolio = new Portfolio()
            {
                TotalCash = 35000,
                AssetAllocations = new List<AssetAllocation>()
                {
                    /*
                    //ETF
                    new AssetAllocation() {Ticker="TQQQ" ,Percentage=10, Ticker_Market=Market.NASDAQ,},
                    new AssetAllocation() {Ticker="SPXL", Percentage = 10, Ticker_Market=Market.NASDAQ, },
                    new AssetAllocation() {Ticker="TNA", Percentage = 10, Ticker_Market=Market.NASDAQ, },
                    new AssetAllocation() {Ticker="MIDU", Percentage = 10, Ticker_Market=Market.NASDAQ, },
                    new AssetAllocation() {Ticker="VNQ", Percentage = 8, Ticker_Market=Market.NASDAQ, },
                    new AssetAllocation() {Ticker="GEX", Percentage = 3, Ticker_Market=Market.NASDAQ, },
                    */

                    //STOCKS
                    new AssetAllocation() {Ticker="AAPL", Percentage = 3, Ticker_Market=Market.NASDAQ, },
                    new AssetAllocation() {Ticker="AMZN", Percentage = 3, Ticker_Market=Market.NASDAQ, },
                    new AssetAllocation() {Ticker="CSCO", Percentage = 3, Ticker_Market=Market.NASDAQ, },
                    new AssetAllocation() {Ticker="GOOGL", Percentage = 3, Ticker_Market=Market.NASDAQ, },
                    new AssetAllocation() {Ticker="IBM", Percentage = 3, Ticker_Market=Market.NYSE, },
                    new AssetAllocation() {Ticker="MSFT", Percentage = 3, Ticker_Market=Market.NASDAQ, },
                    new AssetAllocation() {Ticker="ORCL", Percentage = 3, Ticker_Market=Market.NYSE, },
                    
                    //others
                    //new AssetAllocation("",),
                    //new AssetAllocation("",),

                },
            };
            portfolios.Add(portfolio);
            return portfolios;
#endif
        }
    }
}