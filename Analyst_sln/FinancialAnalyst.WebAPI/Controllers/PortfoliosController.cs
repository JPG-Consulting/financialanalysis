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
            Portfolio portfolio;
            portfolio = Portfolio.From("Warren Buffet", new string[] { 
                "AAL","AAPL","AMZN","AXP","AXTA","BAC","BIIB","BK","CHTR","COST","DAL","DVA","GL","GM","GS","JNJ","JPM",
                "KHC","KO","KR","LBTYA","LBTYK","LILA","LILAK","LSXMA","LSXMK","LUV","MA","MCO","MDLZ","MTB","OXY","PG","PNC","PSX",
                "RH","QSR","SIRI","SPY","STNE","STOR","SU","SYF","TEVA","TRV","UAL","UPS","USB","V","VOO","VRSN","WFC",});
            portfolios.Add(portfolio);
            portfolio = Portfolio.From("Warren Buffet - My Selection", new string[] {
                "AAL","AAPL","AMZN","AXP","AXTA","BAC","BIIB","BK","CHTR","COST","DAL","DVA","GL","GM","GS","JNJ","JPM",
                "KHC","KO","KR","LBTYA","LBTYK","LILA","LILAK","LSXMA","LSXMK","LUV","MA","MCO","MDLZ","MTB","OXY","PG","PNC","PSX",
                "RH","QSR","SIRI","SPY","STNE","STOR","SU","SYF","TEVA","TRV","UAL","UPS","USB","V","VOO","VRSN","WFC",});
            portfolios.Add(portfolio);
            portfolio = Portfolio.From("ETF", new string[] { "TQQQ", "SPXL", "TNA", "MIDU", "VNQ", "GEX", });
            portfolios.Add(portfolio);
            portfolio = Portfolio.From("Big Technologies", new string[] { "AAPL", "AMZN", "CSCO", "GOOGL", "IBM", "MSFT", "ORCL", });
            portfolios.Add(portfolio);
            return portfolios;
#endif
        }
    }
}