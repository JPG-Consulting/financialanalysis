using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Markets
{
    public class Market
    {
        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public string Code { get; set; }

        [JsonProperty]
        public string Description { get; set; }
    }
}
