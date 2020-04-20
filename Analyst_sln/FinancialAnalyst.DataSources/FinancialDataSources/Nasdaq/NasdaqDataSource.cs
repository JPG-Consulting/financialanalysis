using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Assets.Options;
using FinancialAnalyst.Common.Entities.Accounting;
using FinancialAnalyst.Common.Entities.Prices;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.DataSources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using System.Globalization;
using System.Net;
using FinancialAnalyst.Common.Entities.Markets;

namespace FinancialAnalyst.DataSources.FinancialDataSources.Nasdaq
{
    public class NasdaqDataSource : IStockDataDataSource, IFinancialDataSource, IOptionChainDataSource, IPricesDataSource, IIndexesDataSource
    {
        private static readonly ILog logger = log4net.LogManager.GetLogger(typeof(NasdaqDataSource));
        private static readonly CultureInfo enUsCultureInfo = new CultureInfo("en-us");

        public bool TryGetStockSummary(string ticker, Exchange? exchange, out Stock asset, out string errorMessage)
        {
            //https://api.nasdaq.com/api/quote/TQQQ/info?assetclass=etf
            //https://api.nasdaq.com/api/quote/AAPL/info?assetclass=stocks
            bool ok = NasdaqApiCaller.GetStockSummary(ticker, exchange, out HttpStatusCode statusCode, out NasdaqResponse nasdaqResponse, out string jsonResponse, out errorMessage);
            throw new NotImplementedException();
        }

        public bool TryGetFinancialData(string ticker,Exchange? exchange, out FinancialStatements financialData, out string errorMessage)
        {
            //Example
            //https://api.nasdaq.com/api/company/AAPL/financials?frequency=1
            bool ok = NasdaqApiCaller.GetFinancialData(ticker, out string jsonResponse, out errorMessage);
            dynamic rawdata = JsonConvert.DeserializeObject(jsonResponse);
            throw new NotImplementedException();
        }

        public bool TryGetOptionsChain(string ticker, Exchange? exchange, out OptionsChain optionChain, out string errorMessage)
        {
            DateTime from = new DateTime(DateTime.Now.Year,DateTime.Now.Month,1);
            DateTime to = new DateTime(DateTime.Now.Year, DateTime.Now.Month, GetLastDay(DateTime.Now.Month));
            bool ok = NasdaqApiCaller.GetOptionChain(ticker, from, to, out string jsonResponse, out errorMessage);

            optionChain = new OptionsChain();
            dynamic rawdata = JsonConvert.DeserializeObject(jsonResponse);
            
            foreach (var optionRow in rawdata.data.optionChainList.rows)
            {
                if(optionRow.call != null)
                {
                    CallOption o = new CallOption()
                    {
                        Symbol = optionRow.call.symbol,
                        Strike = optionRow.call.strike,
                        ExpirationDate = optionRow.call.expiryDate,
                    };
                    o.SetLast(optionRow.call.last);
                    o.SetChange(optionRow.call.change);
                    o.SetBid(optionRow.call.bid);
                    o.SetAsk(optionRow.call.ask);
                    o.SetVolume(optionRow.call.volume);
                    o.SetOpenInterest(optionRow.call.openinterest);

                    optionChain.Add(o);
                    
                }

                if (optionRow.put != null)
                {
                    PutOption o = new PutOption()
                    {
                        Symbol = optionRow.put.symbol,
                        Last = optionRow.put.last,
                        Strike = optionRow.put.strike,
                        ExpirationDate = optionRow.put.expiryDate,
                    };
                    o.SetLast(optionRow.call.last);
                    o.SetChange(optionRow.call.change);
                    o.SetBid(optionRow.call.bid);
                    o.SetAsk(optionRow.call.ask);
                    o.SetVolume(optionRow.call.volume);
                    o.SetOpenInterest(optionRow.call.openinterest);

                    optionChain.Add(o);
                }
            }

            errorMessage = "ok";
            return true;
        }

        public bool TryGetOptionsChainWithTheoricalValue(string ticker, Exchange? exchange, double lastPrice, out OptionsChain optionsChain, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool TryGetOptionsChainWithTheoricalValue(string ticker, Exchange? exchange, double lastPrice, PriceList historicalPrices, out OptionsChain optionsChain, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool TryGetPrices(string ticker, Exchange? exchange, DateTime? from, DateTime? to, PriceInterval interval, out PriceList prices, out string errorMessage)
        {
            //https://api.nasdaq.com/api/quote/AAPL/chart?assetclass=stocks&fromdate=2010-04-15&todate=2020-04-15
            if (from.HasValue == false)
                from = DateTime.Now;

            if (to.HasValue == false)
                to = DateTime.Now;

            bool ok = NasdaqApiCaller.GetPrices(ticker, exchange, from.Value, to.Value, interval, out HttpStatusCode statusCode, out NasdaqResponse nasdaqResponse, out string jsonResponse, out errorMessage);
            prices = new PriceList();
            foreach(var nasdaqPrice in nasdaqResponse.Data.Prices)
            {
                Price price = new Price();
                price.Close = nasdaqPrice.Price;
                prices.Add(price);
            }
            return true;
        }

        public bool TryGetLastPrice(string ticker, Exchange? exchange, out LastPrice lastPrice, out string message)
        {
            //https://api.nasdaq.com/api/quote/TQQQ/info?assetclass=etf
            //https://api.nasdaq.com/api/quote/AAPL/info?assetclass=stocks
            bool ok = NasdaqApiCaller.GetStockSummary(ticker, exchange, out HttpStatusCode statusCode, out NasdaqResponse nasdaqResponse, out string jsonResponse, out message);
            if(statusCode == HttpStatusCode.OK)
            {
                if (nasdaqResponse.Status.Code == 200)
                {
                    string temp = nasdaqResponse.Data.PrimaryData.LastSalePriceAsString;
                    lastPrice = new LastPrice();
                    lastPrice.Price = decimal.Parse(temp.Substring(1),enUsCultureInfo);
                    temp = nasdaqResponse.Data.PrimaryData.LastTradeTimestampAsString;
                    lastPrice.TimeStamp = ParseDateTime(temp);
                    /*
                    temp = rawdata.data.keyStats.Volume.value.ToString();
                    lastPrice.Volume = int.Parse(temp, NumberStyles.Integer | NumberStyles.AllowThousands, enUsCultureInfo);
                    temp = rawdata.data.keyStats.PreviousClose.value.ToString();
                    lastPrice.PreviousClose = decimal.Parse(temp.Substring(1), enUsCultureInfo);
                    */

                    return true;
                }
                else if(nasdaqResponse.Status.Code == 400 && nasdaqResponse.Status.CodeMessage.Count > 0)
                {
                    lastPrice = null;
                    var nasdaqMessage = nasdaqResponse.Status.CodeMessage[0];
                    message = $"{nasdaqMessage.ErrorMessage} (Code={nasdaqMessage.Code})";
                    return false;
                }

                
            }
            lastPrice = null;
            return false;
        }

        public bool TryGetIndexData(MarketIndex index, out Dictionary<string, decimal> tickersProportions, out string message)
        {
            /*
             * Nasdaq 100
             *      Webs
             *      https://www.nasdaq.com/market-activity/quotes/nasdaq-ndx-index
             *      
             *      Data
             *      https://api.nasdaq.com/api/quote/list-type/nasdaq100
             * 
             */

            throw new NotImplementedException();
        }

        private DateTime ParseDateTime(dynamic timestamp)
        {
            //https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings

            string strTimeStamp = (string)timestamp;
            strTimeStamp = strTimeStamp.Replace("DATA AS OF ","");
            strTimeStamp = strTimeStamp.Replace(" - AFTER HOURS", "");
            TimeZoneInfo timezone = null;
            if (strTimeStamp.Contains(" ET"))
            {
                strTimeStamp = strTimeStamp.Replace(" ET", "");
                timezone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            }

            DateTime dt;
            bool ok = DateTime.TryParseExact(strTimeStamp, "MMM dd, yyyy h:mm tt", enUsCultureInfo, DateTimeStyles.None, out DateTime result1);
            if (ok)
            {
                if (timezone != null)
                {
                    DateTime utcTime = TimeZoneInfo.ConvertTimeToUtc(result1, timezone);
                }
                dt = result1;
            }
            else
            {
                ok = DateTime.TryParseExact(strTimeStamp, "MMM dd, yyyy", enUsCultureInfo, DateTimeStyles.None, out DateTime result2);
                if (ok)
                    dt = result2;
                else
                    dt = DateTime.MaxValue;
                    
            }
            return dt;
        }

        private int GetLastDay(int month)
        {
            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    return 31;
                case 4:
                case 6:
                case 9:
                case 11:
                    return 30;
                case 2:
                    return 28;
                default:
                    throw new NotImplementedException();
            }
        }

        
    }
}
