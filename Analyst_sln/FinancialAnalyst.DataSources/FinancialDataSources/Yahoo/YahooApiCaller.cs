using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Prices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace FinancialAnalyst.DataSources.FinancialDataSources.Yahoo
{
    internal class YahooApiCaller
    {
        private static readonly HttpClient httpClientHistoricalPrices = new HttpClient()
        {
            BaseAddress = new Uri("https://query1.finance.yahoo.com/v7/finance/download"),
            Timeout = TimeSpan.FromMinutes(5),
        };

        private static readonly HttpClient httpClientChartPrices = new HttpClient()
        {
            BaseAddress = new Uri("https://query1.finance.yahoo.com/v8/finance/chart"),
            Timeout = TimeSpan.FromMinutes(5),
        };

        private static readonly HttpClient httpClientQuote = new HttpClient()
        {
            BaseAddress = new Uri("https://query2.finance.yahoo.com/v7/finance/quote"),
            Timeout = TimeSpan.FromMinutes(5),
        };

        /*
        private static readonly HttpClient httpClientSummary = new HttpClient() { BaseAddress = new Uri("https://query1.finance.yahoo.com/v10/finance/quoteSummary") };
        
        private static readonly HttpClient httpClientFundamentals = new HttpClient() { BaseAddress = new Uri("https://query2.finance.yahoo.com/ws/fundamentals-timeseries/v1/finance/timeseries/") };
        */

        internal static bool GetQuoteData(string ticker, out HttpStatusCode statusCode, out YahooQuoteResponse yahooResponse, out string jsonResponse, out string message)
        {
            //Example 1
            //https://query2.finance.yahoo.com/v7/finance/quote?formatted=true&lang=en-US&region=US&symbols=AAPL

            //Example 2
            //https://query2.finance.yahoo.com/v7/finance/quote?formatted=true&lang=en-US&region=US&symbols=TQQQ

            string uri = $"{httpClientQuote.BaseAddress}?formatted=true&lang=en-US&region=US&symbols={ticker}";
            HttpResponseMessage responseMessage = httpClientQuote.GetAsync(uri).Result;
            string content = responseMessage.Content.ReadAsStringAsync().Result;
            statusCode = responseMessage.StatusCode;
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                jsonResponse = content;
                yahooResponse = JsonConvert.DeserializeObject<YahooQuoteResponse>(jsonResponse);
                message = "OK";
                return true;
            }
            else
            {
                dynamic error = new
                {
                    HttpStatusCode = responseMessage.StatusCode.ToString(),
                    ReasonPhrase = responseMessage.ReasonPhrase,
                    ContentResponse = content,
                };
                jsonResponse = JsonConvert.SerializeObject(error);
                yahooResponse = null;
                message = responseMessage.ReasonPhrase;
                return false;
            }
        }

        internal static bool GetDailyPrices(string ticker,YahooChartInterval interval, int days, out HttpStatusCode statusCode, out YahooChartResponse yahooResponse, out string jsonResponse, out string message)
        {
            //Last two months???
            //https://query1.finance.yahoo.com/v8/finance/chart/AAPL?region=US&lang=en-US&includePrePost=false&interval=2m&range=1d&corsDomain=finance.yahoo.com&.tsrc=finance

            //Daily, last day
            //https://query1.finance.yahoo.com/v8/finance/chart/AAPL?lang=en-US&includePrePost=false&interval=1d&range=1d

            //Daily, last 5 days
            //https://query1.finance.yahoo.com/v8/finance/chart/AAPL?lang=en-US&includePrePost=false&interval=1d&range=5d

            //Wrong parameter
            //https://query1.finance.yahoo.com/v8/finance/chart/AAPL?region=US&lang=en-US&includePrePost=false&interval=5h&range=1d
            //{"chart":{"result":null,"error":{"code":"Bad Request","description":"Invalid input - interval=5h is not supported. Valid intervals: [1m, 2m, 5m, 15m, 30m, 60m, 90m, 1h, 1d, 5d, 1wk, 1mo, 3mo]"}}}

            //Daily, every 1 minutes
            //https://query1.finance.yahoo.com/v8/finance/chart/AAPL?region=US&lang=en-US&includePrePost=false&interval=1m&range=1d

            //Daily, every 15 minutes
            //https://query1.finance.yahoo.com/v8/finance/chart/AAPL?region=US&lang=en-US&includePrePost=false&interval=15m&range=1d

            string uri = $"{httpClientChartPrices.BaseAddress}/{ticker}?lang=en-US&includePrePost=false&interval={interval}&range={days}d";
            HttpResponseMessage responseMessage = httpClientChartPrices.GetAsync(uri).Result;
            string content = responseMessage.Content.ReadAsStringAsync().Result;
            statusCode = responseMessage.StatusCode;
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                jsonResponse = content;
                yahooResponse = JsonConvert.DeserializeObject<YahooChartResponse>(jsonResponse);
                message = "OK";
                return true;
            }
            else
            {
                dynamic error = new
                {
                    HttpStatusCode = responseMessage.StatusCode.ToString(),
                    ReasonPhrase = responseMessage.ReasonPhrase,
                    ContentResponse = content,
                };
                jsonResponse = JsonConvert.SerializeObject(error);
                yahooResponse = null;
                message = responseMessage.ReasonPhrase;
                return false;
            }
        }

        internal static string GetHistoricalPrices(string ticker, double from, double to, PriceInterval priceInterval)
        {
            //daily
            //https://query1.finance.yahoo.com/v7/finance/download/%5EGSPC?period1=-1325635200&period2=1584144000&interval=1d&events=history

            //monthly
            //https://query1.finance.yahoo.com/v7/finance/download/%5EGSPC?period1=-1325635200&period2=1584144000&interval=1mo&events=history

            string interval = Translate(priceInterval);
            string requestUrl = $"{httpClientHistoricalPrices.BaseAddress}/{ticker}?period1={(long)from}&period2={(long)to}&interval={interval}&events=history";

            //var content = new KeyValuePair<string, string>[] {
            //    };
            //var formUrlEncodedContent = new FormUrlEncodedContent(content);

            using (var request = new HttpRequestMessage(HttpMethod.Get, requestUrl))
            {
                var sendTask = httpClientHistoricalPrices.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                var response = sendTask.Result.EnsureSuccessStatusCode();
                var httpStream = response.Content.ReadAsStreamAsync().Result;


                using (MemoryStream ms = new MemoryStream())
                {
                    httpStream.CopyTo(ms);
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }
        
        private static string Translate(PriceInterval priceInterval)
        {
            switch (priceInterval)
            {
                case PriceInterval.Daily: return "1d";
                case PriceInterval.Monthly: return "1mo";
                default:
                    throw new NotImplementedException($"There is no interval configured for value={priceInterval.ToString()}");
            }
        }


    }
}
