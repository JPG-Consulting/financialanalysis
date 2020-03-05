using FinancialAnalyst.Common.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace FinancialAnalyst.DataSources.Yahoo
{
    public class YahooWebAPI
    {
        private static readonly HttpClient httpClientSummary = new HttpClient() { BaseAddress = new Uri("https://query1.finance.yahoo.com/v10/finance/quoteSummary") };

        private static readonly HttpClient httpClientBasicData = new HttpClient() { BaseAddress = new Uri("https://query2.finance.yahoo.com/v7/finance/quote") };
        
        private static readonly HttpClient httpClientFundamentals = new HttpClient() { BaseAddress = new Uri("https://query2.finance.yahoo.com/ws/fundamentals-timeseries/v1/finance/timeseries/") };

        internal static dynamic GetBasicData(string ticker,Exchange market)
        {
            throw new NotImplementedException();
        }
    }
}
