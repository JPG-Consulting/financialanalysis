using FinancialAnalyst.Common.Entities.Assets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Portfolios
{
    [Serializable]
    public class AssetAllocation
    {
        public int Id { get; set; }

        [JsonProperty]
        public string Ticker { get; set; }

        [JsonProperty]
        public Exchange? Exchange { get; set; }
        
        [JsonProperty]
        public int? Quantity { get; set; }

        [JsonProperty]
        public decimal? Costs { get; set; }

        [JsonProperty]
        public decimal? MarketValue { get; set; }

        [JsonProperty]
        public decimal? Percentage { get; set; }

        public int PortfolioId { get; set; }

        /// <summary>
        /// It updates the market value if there is quantity.
        /// </summary>
        /// <param name="price"></param>
        public void CalculateMarketValue(decimal price)
        {
            if (Quantity.HasValue)
            {
                MarketValue = Quantity.Value * price;
            }
        }
    }
}
