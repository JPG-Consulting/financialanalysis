using FinancialAnalyst.Common.Entities.Assets.Options;
using FinancialAnalyst.Common.Entities.Prices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Assets
{
    [Serializable]
    public abstract class AssetBase
    {
        [JsonProperty]
        [Key]
        public int Id { get; set; }

        [JsonProperty]
        [Required]
        public string Ticker { get; private set; }

        [JsonProperty]
        public Exchange? Exchange { get; set; }

        [JsonProperty]
        public abstract AssetClass AssetClass { get; protected set; }

        [JsonProperty]
        public decimal? LastPrice { get; set; }

        [JsonProperty]
        public DateTime? LastPrice_Date { get; set; }

        [JsonProperty]
        public List<HistoricalPrice> HistoricalPrices { get; set; }

        public AssetBase(string ticker)
        {
            if (string.IsNullOrEmpty(ticker))
                throw new ArgumentException("Ticker can't be null neither empty");
            Ticker = ticker;
        }

    }
}
