using FinancialAnalyst.Common.Entities.Assets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Portfolios
{
    [Serializable]
    public class Portfolio
    {
        //[JsonArray]
        public List<AssetAllocation> AssetAllocations { get; set; }

        [JsonProperty]
        public decimal TotalCash { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        public static Portfolio From(string name, string[] tickers)
        {
            Portfolio p = new Portfolio()
            {
                Name = name,
                AssetAllocations = new List<AssetAllocation>(),
            };
            foreach(string ticker in tickers)
            {
                p.AssetAllocations.Add(new AssetAllocation()
                {
                    Ticker=ticker,
                });
            }
            return p;
        }
    }
}
