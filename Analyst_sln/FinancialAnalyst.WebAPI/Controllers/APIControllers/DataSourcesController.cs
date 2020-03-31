using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Prices;
using FinancialAnalyst.Common.Entities.RequestResponse;
using FinancialAnalyst.Common.Interfaces;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.DataSources;
using FinancialAnalyst.WebAPI.Models;
using FinancialAnalyst.WebAPI.Properties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FinancialAnalyst.WebAPI.Controllers.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataSourcesController : ControllerBase
    {
        private IDataSource dataSource;
        private IPricesDataSource pricesDataSource;

        public DataSourcesController(IDataSource dataSourceManager, IPricesDataSource pricesDataSource)
        {
            this.dataSource = dataSourceManager;
            this.pricesDataSource = pricesDataSource;
        }

        [HttpGet("getstockdata")]
        public APIResponse<Stock> GetStockData(string ticker,string exchange)
        {

            try
            {
                if (string.IsNullOrEmpty(ticker))
                {
                    return new APIResponse<Stock>()
                    {
                        Ok = false,
                        ErrorMessage = Resources.UI_TickerNull,
                    };
                }

                Exchange? exch = null;
                if (string.IsNullOrEmpty(exchange) == false)
                {
                    if (Enum.TryParse<Exchange>(exchange, out Exchange temp))
                    {
                        exch = temp;
                    }
                    else
                    {
                        return new APIResponse<Stock>()
                        {
                            Content = null,
                            Ok = false,
                            ErrorMessage = $"Market {exchange} is not a valid market",
                        };
                    }
                }

                if (dataSource.TryGetCompleteStockData(ticker, exch, out Stock stock, out string message))
                {
                    return new APIResponse<Stock>()
                    {
                        Content = stock,
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
            catch(Exception ex)
            {
                return new APIResponse<Stock>()
                {
                    Content = null,
                    Ok = false,
                    ErrorMessage = ex.Message,
                };
            }
        }


        [HttpGet("getprices")]
        public APIResponse<PriceList> GetPrices(string ticker, string exchange, string from, string to, string interval)
        {
            if (string.IsNullOrEmpty(ticker))
            {
                return new APIResponse<PriceList>()
                {
                    Ok = false,
                    ErrorMessage = Resources.UI_TickerNull,
                };
            }

            Exchange? exch = null;
            if (string.IsNullOrEmpty(exchange) == false)
            {
                if (Enum.TryParse<Exchange>(exchange, out Exchange exchangeValue))
                {
                    exch = exchangeValue;
                }
                else
                {
                    return new APIResponse<PriceList>()
                    {
                        Content = null,
                        Ok = false,
                        ErrorMessage = $"Market {exchange} is not a valid market",
                    };
                }
            }


            DateTime? fromDate = null;
            if (string.IsNullOrEmpty(from) == false && DateTime.TryParse(from, out DateTime temp) == true)
            {
                fromDate = temp;
            }

            DateTime? toDate = null;
            if (string.IsNullOrEmpty(to) == false && DateTime.TryParse(to, out temp) == true)
            {
                toDate = temp;
            }

            if(Enum.TryParse<PriceInterval>(interval,out PriceInterval priceInterval) == false)
            {
                return new APIResponse<PriceList>()
                {
                    Content = null,
                    Ok = false,
                    ErrorMessage = $"PriceInterval '{interval}' is not a valid interval",
                };
            }

            if (dataSource.TryGetPrices(ticker, exch,fromDate,toDate, priceInterval, out PriceList prices, out string message))
            {
                return new APIResponse<PriceList>()
                {
                    Content = (PriceList)prices,
                    Ok = true,
                    ErrorMessage = message,
                };
            }
            else
            {
                return new APIResponse<PriceList>()
                {
                    Content = null,
                    Ok = false,
                    ErrorMessage = message,
                };
            }

        }
    }
}