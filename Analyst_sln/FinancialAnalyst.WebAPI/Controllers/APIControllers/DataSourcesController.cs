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
        //https://docs.microsoft.com/es-es/aspnet/core/web-api/?view=aspnetcore-3.1

        private IDataSource dataSource;
        private IPricesDataSource pricesDataSource;

        public DataSourcesController(IDataSource dataSourceManager, IPricesDataSource pricesDataSource)
        {
            this.dataSource = dataSourceManager;
            this.pricesDataSource = pricesDataSource;
        }

        [HttpGet("getcompletestockdata")]
        public APIResponse<AssetBase> GetCompleteStockData(string ticker,string exchange,string assetType, bool? includeOptionChain, bool? includeFinancialStatements )
        {

            try
            {
                if (string.IsNullOrEmpty(ticker))
                {
                    return new APIResponse<AssetBase>()
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
                        return new APIResponse<AssetBase>()
                        {
                            Content = null,
                            Ok = false,
                            ErrorMessage = $"Market {exchange} is not a valid market",
                        };
                    }
                }

                AssetClass assetTypeValue = AssetClass.Unknown;
                if (string.IsNullOrEmpty(assetType) == false)
                {
                    if (Enum.TryParse<Exchange>(assetType, out Exchange temp))
                    {
                        exch = temp;
                    }
                    else
                    {
                        return new APIResponse<AssetBase>()
                        {
                            Content = null,
                            Ok = false,
                            ErrorMessage = $"Asset type {assetType} is not a asset type",
                        };
                    }
                }


                if (includeOptionChain == null)
                    includeOptionChain = false;

                if (includeFinancialStatements == null)
                    includeFinancialStatements = false;

                if (dataSource.TryGetCompleteAssetData(ticker, exch, assetTypeValue, includeOptionChain.Value, includeFinancialStatements.Value, out AssetBase asset, out string message))
                {
                    return new APIResponse<AssetBase>()
                    {
                        Content = asset,
                        Ok = true,
                        ErrorMessage = message,
                    };
                }
                else
                {
                    return new APIResponse<AssetBase>()
                    {
                        Content = null,
                        Ok = false,
                        ErrorMessage = message,
                    };
                }
            }
            catch(Exception ex)
            {
                return new APIResponse<AssetBase>()
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

            if (dataSource.TryGetHistoricalPrices(ticker, exch,fromDate,toDate, priceInterval, out PriceList prices, out string message))
            {
                return new APIResponse<PriceList>()
                {
                    Content = prices,
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

        [HttpGet("getlastprice")]
        public APIResponse<HistoricalPrice> GetLastPrice(string ticker, string exchange, string assetType)
        {
            if (string.IsNullOrEmpty(ticker))
            {
                return new APIResponse<HistoricalPrice>()
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
                    return new APIResponse<HistoricalPrice>()
                    {
                        Content = null,
                        Ok = false,
                        ErrorMessage = $"Market {exchange} is not a valid market",
                    };
                }
            }

            AssetClass assetTypeValue = AssetClass.Unknown;
            if (string.IsNullOrEmpty(assetType) == false)
            {
                if (Enum.TryParse<Exchange>(assetType, out Exchange temp))
                {
                    exch = temp;
                }
                else
                {
                    return new APIResponse<HistoricalPrice>()
                    {
                        Content = null,
                        Ok = false,
                        ErrorMessage = $"Asset type {assetType} is not a asset type",
                    };
                }
            }

            if (dataSource.TryGetLastPrice(ticker, exch, assetTypeValue, out HistoricalPrice price, out string message))
            {
                return new APIResponse<HistoricalPrice>()
                {
                    Content = price,
                    Ok = true,
                    ErrorMessage = message,
                };
            }
            else
            {
                return new APIResponse<HistoricalPrice>()
                {
                    Content = null,
                    Ok = false,
                    ErrorMessage = message,
                };
            }
        }
    }
}