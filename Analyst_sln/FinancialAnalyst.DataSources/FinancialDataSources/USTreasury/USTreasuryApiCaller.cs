using Newtonsoft.Json;
using System;
using System.Net.Http;


namespace FinancialAnalyst.DataSources.USTreasury
{
    /// <summary>
    /// https://www.treasury.gov/resource-center/data-chart-center/interest-rates/pages/TextView.aspx?data=yield
    /// </summary>
    internal class USTreasuryApiCaller
    {
        private static readonly HttpClient httpClient = new HttpClient() { BaseAddress = new Uri("http://data.treasury.gov/feed.svc/") };

        /// <summary>
        /// http://data.treasury.gov/feed.svc/DailyTreasuryYieldCurveRateData
        /// </summary>
        /// <param name="ticker"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="jsonResponse"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        internal static bool GetDailyTreasuryYieldCurveRateData(int year, int month, out string xmlResponse, out string errorMessage)
        {
            //http://data.treasury.gov/feed.svc/DailyTreasuryYieldCurveRateData?$filter=month(NEW_DATE) eq 3 and year(NEW_DATE) eq 2020
            //https://data.treasury.gov/feed.svc/DailyTreasuryYieldCurveRateData?$filter=month(NEW_DATE)%20eq%203%20and%20year(NEW_DATE)%20eq%202020


            string uri = $"{httpClient.BaseAddress}DailyTreasuryYieldCurveRateData?$filter=month(NEW_DATE) eq {month} and year(NEW_DATE) eq {year}";
            HttpResponseMessage responseMessage = httpClient.GetAsync(uri).Result;
            string originalContent = responseMessage.Content.ReadAsStringAsync().Result;
            string content = originalContent;
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                errorMessage = "OK";
                xmlResponse = content;
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
                xmlResponse = error.ToXml();
                errorMessage = responseMessage.ReasonPhrase;
                return false;
            }
        }
    }

    
}
