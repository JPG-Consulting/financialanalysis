using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Prices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace FinancialAnalyst.DataSources.FinancialDataSources.Nasdaq
{
    internal class NasdaqApiCaller
    {
        private static readonly HttpClient httpClient = new HttpClient() { BaseAddress = new Uri("https://api.nasdaq.com/api/") };

        internal static bool GetStockSummary(string ticker, Exchange? exchange, AssetType assetType, out HttpStatusCode statusCode, out NasdaqResponse nasdaqResponse, out string jsonResponse, out string message)
        {
            //https://api.nasdaq.com/api/quote/TQQQ/info?assetclass=etf
            //https://api.nasdaq.com/api/quote/AAPL/info?assetclass=stocks
            string uri = $"{httpClient.BaseAddress}quote/{ticker}/info";
            if(assetType == AssetType.Stock)
                uri += "?assetclass=stocks";
            else if(assetType == AssetType.ETF)
                uri += "?assetclass=etf";
            HttpResponseMessage responseMessage = httpClient.GetAsync(uri).Result;
            string content = responseMessage.Content.ReadAsStringAsync().Result;
            statusCode = responseMessage.StatusCode;
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                jsonResponse = content;
                nasdaqResponse = JsonConvert.DeserializeObject<NasdaqResponse>(jsonResponse);
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
                nasdaqResponse = null;
                message = responseMessage.ReasonPhrase;
                return false;
            }
        }

        internal static bool GetFinancialData(string ticker, out string jsonResponse, out string errorMessage)
        {
            //https://api.nasdaq.com/api/company/GM/financials?frequency=1
            //https://api.nasdaq.com/api/company/AMZN/financials?frequency=1
            //https://api.nasdaq.com/api/company/AAPL/financials?frequency=1

            string uri = $"{httpClient.BaseAddress}company/{ticker}/financials?frequency=1";
            HttpResponseMessage responseMessage = httpClient.GetAsync(uri).Result;
            string originalContent = responseMessage.Content.ReadAsStringAsync().Result;
            string content = originalContent;
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                errorMessage = "OK";
                jsonResponse = content;
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
                errorMessage = responseMessage.ReasonPhrase;
                return false;
            }
        }

        internal static bool GetOptionChain(string ticker,DateTime from, DateTime to, out string jsonResponse, out string errorMessage)
        {
            //https://api.nasdaq.com/api/quote/GM/option-chain?assetclass=stocks&todate=2020-03-31&fromdate=2020-03-01&limit=0

            string strFrom = from.ToString("yyyy-MM-dd", null);
            string strTo = to.ToString("yyyy-MM-dd", null);
            string uri = $"{httpClient.BaseAddress}quote/{ticker}/option-chain?assetclass=stocks&todate={strTo}&fromdate={strFrom}&limit=0";
            HttpResponseMessage responseMessage = httpClient.GetAsync(uri).Result;
            string originalContent = responseMessage.Content.ReadAsStringAsync().Result;
            string content = originalContent;
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                errorMessage = "OK";
                jsonResponse = content;
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
                errorMessage = responseMessage.ReasonPhrase;
                return false;
            }
        }

        internal static bool GetPrices(string ticker, Exchange? exchange, DateTime from, DateTime to, PriceInterval interval, out HttpStatusCode statusCode, out NasdaqResponse nasdaqResponse, out string jsonResponse, out string message)
        {
            //https://api.nasdaq.com/api/quote/AAPL/chart?assetclass=stocks&fromdate=2010-04-15&todate=2020-04-15
            string fromDate = from.ToString("yyyy-MM-dd");
            string toDate = to.ToString("yyyy-MM-dd");
            string uri = $"{httpClient.BaseAddress}quote/{ticker}/chart?assetclass=stocks&fromdate={fromDate}5&todate={toDate}";
            HttpResponseMessage responseMessage = httpClient.GetAsync(uri).Result;
            string originalContent = responseMessage.Content.ReadAsStringAsync().Result;
            string content = originalContent;
            statusCode = responseMessage.StatusCode;
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                jsonResponse = content;
                nasdaqResponse = JsonConvert.DeserializeObject<NasdaqResponse>(jsonResponse);
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
                nasdaqResponse = null;
                message = responseMessage.ReasonPhrase;
                return false;
            }
        }
    }
}
