using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Portfolios;
using FinancialAnalyst.Common.Entities.Prices;
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

        private string[] permittedExtensions = { ".csv", };

        private readonly IPortfoliosManager portfoliosService;
        public PortfoliosController(IPortfoliosManager portfoliosService)
        {
            this.portfoliosService = portfoliosService;
        }

        [HttpGet("getportfoliosbyuser")]
        public ActionResult<APIResponse<IEnumerable<Portfolio>>> GetPortfoliosByUser(string username)
        {
            bool ok = portfoliosService.GetPortfoliosByUserName(username, out IEnumerable<Portfolio> portfolios, out string message);
            APIResponse<IEnumerable<Portfolio>> response = new APIResponse<IEnumerable<Portfolio>>()
            {
                Content = portfolios,
                Ok = ok,
                ErrorMessage = message,
            };
            if (ok)
                return StatusCode(StatusCodes.Status200OK, response);
            else
                return StatusCode(StatusCodes.Status422UnprocessableEntity, response);
        }

        [HttpPost("addassetallocation")]
        public ActionResult<APIResponse<Portfolio>> AddAssetAllocation(string username, int portfolioId, string symbol, decimal amount)
        {
            APIResponse<Portfolio> response = new APIResponse<Portfolio>()
            {
                Content = null,
                Ok = false,
                ErrorMessage = "Not implemented",
            };
            return StatusCode(StatusCodes.Status501NotImplemented, response);
        }

        [HttpPost("updateassetallocation")]
        public ActionResult<APIResponse<AssetAllocation>> UpdateAssetAllocation([FromBody]AssetAllocation assetAllocation)
        {
            bool ok = portfoliosService.Update(assetAllocation, out decimal? marketValue);
            return Ok(new APIResponse<AssetAllocation>()
            {
                Content = assetAllocation,
                Ok = ok,
                ErrorMessage = "<pending>",
            });
        }

        [HttpPost("createportfolio")]
        public ActionResult<APIResponse<Portfolio>> CreatePortfolio([FromForm]string username, [FromForm] string portfolioname, [FromForm] IFormFile transactions, [FromForm] bool firstRowIsInitalBalance=true)
        {
            //https://docs.microsoft.com/es-es/aspnet/core/mvc/models/file-uploads?view=aspnetcore-3.1

            bool overrideIfExist = false;

            MemoryStream target = new MemoryStream();
            transactions.CopyTo(target);
            bool ok = portfoliosService.CreatePortfolio(username, portfolioname,target, firstRowIsInitalBalance, overrideIfExist, out Portfolio portfolio, out string message);
            APIResponse<Portfolio> response = new APIResponse<Portfolio>()
            {
                Content = portfolio,
                Ok = ok,
                ErrorMessage = message,
            };
            if (ok)
                return StatusCode(StatusCodes.Status200OK, response);
            else
                return StatusCode(StatusCodes.Status422UnprocessableEntity, response);
        }

        [HttpPost("updateportfolio")]
        public ActionResult<APIResponse<bool>> UpdatePortfolio([FromForm]string id, [FromForm]string marketValue)
        {
            try
            {
                int iId = int.Parse(id);
                decimal dPortfolioMarketValue = decimal.Parse(marketValue);
                bool ok = portfoliosService.Update(iId,dPortfolioMarketValue);
                APIResponse<bool> response = new APIResponse<bool>()
                {
                    Content = ok,
                    Ok = ok,
                    ErrorMessage = "ok",
                };
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch(Exception ex)
            {
                APIResponse<bool> response = new APIResponse<bool>()
                {
                    Content = false,
                    Ok = false,
                    ErrorMessage = ex.Message,
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


    }
}