using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Prices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Portfolios
{
    [JsonObject]
    [Serializable]
    public class AssetAllocation
    {
        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public string Ticker { get; set; }

        [JsonProperty]
        public AssetType? AssetType { get; set; }

        [JsonProperty]
        public Exchange? Exchange { get; set; }
        
        [JsonProperty]
        public int? Quantity { get; set; }

        [JsonProperty]
        public decimal? Costs { get; set; }

        [JsonProperty]
        public decimal? Price { get; set; }

        [JsonProperty]
        public DateTime? PriceDate { get; set; }


        [JsonProperty]
        public decimal? TheoricalPrice { get; set; }

        [JsonProperty]
        public DateTime? TheoricalPriceDate { get; set; }

        [IgnoreDataMember]
        [JsonProperty]
        public decimal? MarketValue
        {
            get
            {
                if (Price.HasValue && Quantity.HasValue)
                    return Price.Value * Quantity.Value;
                else
                    return null;
            }
        }


        [IgnoreDataMember]
        [JsonProperty]
        public decimal? TheoricalMarketValue
        {
            get
            {
                if (TheoricalPrice.HasValue && Quantity.HasValue)
                    return TheoricalPrice.Value * Quantity.Value;
                else
                    return null;
            }
        }

        [JsonProperty]
        public decimal? Percentage { get; set; }

        [JsonProperty]
        public int PortfolioId { get; set; }

        /// <summary>
        /// It updates the market value if there is quantity.
        /// </summary>
        /// <param name="price"></param>
        public decimal? UpdateMarketValue(decimal price, DateTime priceDate)
        {
            Price = price;
            PriceDate = priceDate;
            return MarketValue;
        }
    }
}
