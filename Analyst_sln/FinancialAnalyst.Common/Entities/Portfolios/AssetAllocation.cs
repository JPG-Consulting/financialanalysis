﻿using FinancialAnalyst.Common.Entities.Assets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Portfolios
{
    [Serializable]
    public class AssetAllocation
    {
        [JsonProperty]
        public string Ticker { get; set; }

        [JsonProperty]
        public Exchange? Exchange { get; set; }

        [JsonProperty]
        public decimal? Amount { get; set; }

        [JsonProperty]
        public decimal? Percentage { get; set; }
    }
}