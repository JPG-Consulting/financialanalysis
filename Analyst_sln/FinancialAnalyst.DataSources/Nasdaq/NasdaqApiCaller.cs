using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace FinancialAnalyst.DataSources.Nasdaq
{
    internal class NasdaqApiCaller
    {
        private static readonly HttpClient httpClient = new HttpClient() { BaseAddress = new Uri("https://api.nasdaq.com/api/quote/") };

        internal static bool GetOptionChain(string ticker,DateTime from, DateTime to, out string jsonResponse, out string errorMessage)
        {
            //https://api.nasdaq.com/api/quote/GM/option-chain?assetclass=stocks&todate=2020-03-31&fromdate=2020-03-01&limit=0

            string strFrom = from.ToString("yyyy-MM-dd", null);
            string strTo = to.ToString("yyyy-MM-dd", null);
            string uri = $"{httpClient.BaseAddress}{ticker}/option-chain?assetclass=stocks&todate={strTo}&fromdate={strFrom}&limit=0";
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
    }
}
