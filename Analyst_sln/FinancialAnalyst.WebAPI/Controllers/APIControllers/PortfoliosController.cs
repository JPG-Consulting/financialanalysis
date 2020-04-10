using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Portfolios;
using FinancialAnalyst.Common.Entities.RequestResponse;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces;
using FinancialAnalyst.DataAccess.Portfolios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinancialAnalyst.WebAPI.Controllers.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfoliosController : ControllerBase
    {
        //https://docs.microsoft.com/es-es/aspnet/core/web-api/?view=aspnetcore-3.1
        //https://www.devtrends.co.uk/blog/handling-errors-in-asp.net-core-web-api

        private readonly IPortfoliosManager portfoliosService;
        public PortfoliosController(IPortfoliosManager portfoliosService)
        {
            this.portfoliosService = portfoliosService;
        }

        [HttpGet("getdefaultportfolios")]
        public ActionResult<APIResponse<IEnumerable<Portfolio>>> GetDefaultPortfolios()
        {
            List<Portfolio> portfolios = new List<Portfolio>();
            Portfolio portfolio;
            portfolio = Portfolio.From("Warren Buffet", new string[] {
                "AAL","AAPL","AMZN","AXP","AXTA","BAC","BIIB","BK","CHTR","COST","DAL","DVA","GL","GM","GS","JNJ","JPM",
                "KHC","KO","KR","LBTYA","LBTYK","LILA","LILAK","LSXMA","LSXMK","LUV","MA","MCO","MDLZ","MTB","OXY","PG","PNC","PSX",
                "RH","QSR","SIRI","SPY","STNE","STOR","SU","SYF","TEVA","TRV","UAL","UPS","USB","V","VOO","VRSN","WFC",});
            portfolios.Add(portfolio);
            portfolio = Portfolio.From("ETF", new string[] { "TQQQ", "SPXL", "TNA", "MIDU", "VNQ", "GEX", });
            portfolios.Add(portfolio);
            portfolio = Portfolio.From("Big Technologies", new string[] { "AAPL", "AMZN", "CSCO", "GOOGL", "IBM", "MSFT", "ORCL", });
            portfolios.Add(portfolio);
            APIResponse<IEnumerable<Portfolio>> response = new APIResponse<IEnumerable<Portfolio>>()
            {
                Content = portfolios,
                Ok = true,
                ErrorMessage = "ok",
            };
            return Ok(response);
        }

        [HttpGet("getportfoliosbyuser")]
        public ActionResult<APIResponse<IEnumerable<Portfolio>>> GetPortfoliosByUser(string userId)
        {
            APIResponse<IEnumerable<Portfolio>> response = new APIResponse<IEnumerable<Portfolio>>()
            {
                Content = null,
                Ok = false,
                ErrorMessage = "Not implemented",
            };
            return StatusCode(StatusCodes.Status501NotImplemented, response);
        }


        private string[] permittedExtensions = { ".csv",};

        [HttpPost("createportfolio")]
        public ActionResult<APIResponse<Portfolio>> CreatePortfolio([FromForm]string username, [FromForm] string portfolioname, [FromForm] IFormFile transactions, [FromForm] bool firstRowIsInitalBalance=true)
        {
            //https://docs.microsoft.com/es-es/aspnet/core/mvc/models/file-uploads?view=aspnetcore-3.1

            MemoryStream target = new MemoryStream();
            transactions.CopyTo(target);
            bool ok = portfoliosService.Create(username, portfolioname,target, firstRowIsInitalBalance, out Portfolio portfolio, out string message);
            if (ok)
            {
                APIResponse<Portfolio> response = new APIResponse<Portfolio>()
                {
                    Content = portfolio,
                    Ok = true,
                    ErrorMessage = message,
                };
                return StatusCode(StatusCodes.Status200OK, response);
            }
            else
            {
                APIResponse<Portfolio> response = new APIResponse<Portfolio>()
                {
                    Content = portfolio,
                    Ok = true,
                    ErrorMessage = message,
                };
                return StatusCode(StatusCodes.Status422UnprocessableEntity, response);
            }
        }
    }
}