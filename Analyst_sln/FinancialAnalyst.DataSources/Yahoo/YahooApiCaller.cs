using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Prices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace FinancialAnalyst.DataSources.Yahoo
{
    internal class YahooApiCaller
    {
        private static readonly HttpClient httpClientSummary = new HttpClient() { BaseAddress = new Uri("https://query1.finance.yahoo.com/v10/finance/quoteSummary") };

        private static readonly HttpClient httpClientPrices = new HttpClient() 
        { 
            BaseAddress = new Uri("https://query1.finance.yahoo.com/v7/finance/download"),
            Timeout = TimeSpan.FromMinutes(5),
        };

        private static readonly HttpClient httpClientBasicData = new HttpClient() { BaseAddress = new Uri("https://query2.finance.yahoo.com/v7/finance/quote") };
        
        private static readonly HttpClient httpClientFundamentals = new HttpClient() { BaseAddress = new Uri("https://query2.finance.yahoo.com/ws/fundamentals-timeseries/v1/finance/timeseries/") };

        internal static dynamic GetBasicData(string ticker,Exchange market)
        {
            throw new NotImplementedException();
        }

        internal static string GetPrices(string ticker, double from, double to, PriceInterval priceInterval)
        {
            //daily
            //https://query1.finance.yahoo.com/v7/finance/download/%5EGSPC?period1=-1325635200&period2=1584144000&interval=1d&events=history

            //monthly
            //https://query1.finance.yahoo.com/v7/finance/download/%5EGSPC?period1=-1325635200&period2=1584144000&interval=1mo&events=history

            string interval = Translate(priceInterval);
            string requestUrl = $"{httpClientPrices.BaseAddress}/{ticker}?period1={(long)from}&period2={(long)to}&interval={interval}&events=history";

            //var content = new KeyValuePair<string, string>[] {
            //    };
            //var formUrlEncodedContent = new FormUrlEncodedContent(content);

            using (var request = new HttpRequestMessage(HttpMethod.Get, requestUrl))
            {
                var sendTask = httpClientPrices.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
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
