using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.RequestResponse;
using FinancialAnalyst.Common.Interfaces;
using FinancialAnalyst.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FinancialAnalyst.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataSourcesController : ControllerBase
    {
        private IDataSource dataSource;

        public DataSourcesController()
        {
            //TODO: add dependency injection
            this.dataSource = new FinancialAnalyst.DataSources.Reuters.ReutersDataSource();
        }

        /*
        public DataSourcesController(IDataSource dataSource)
        {
            //TODO: add dependency injection
            this.dataSource = dataSource;
        }
        */

        [HttpGet("getassetdata")]
        public APIResponse<Stock> GetAssetData(string ticker,string market)
        {
            Market m;
            if (Enum.TryParse<Market>(market, out m))
            {
                if (dataSource.TryGetAssetData(ticker, m, out AssetBase asset, out string message))
                {
                    return new APIResponse<Stock>()
                    {
                        Content = (Stock)asset,
                        Ok = true,
                        ErrorMessage = message,
                    };
                }
                else
                {
                    return new APIResponse<Stock>()
                    {
                        Content = null,
                        Ok = false,
                        ErrorMessage = message,
                    };
                }
            }
            else
            {
                return new APIResponse<Stock>()
                {
                    Content = null,
                    Ok = false,
                    ErrorMessage = $"Market {market} is not a valid market",
                };
            }
        }

    }
}