using FinancialAnalyst.Common.Entities.Prices;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Assets
{
    [JsonDictionary]
    public class OptionsChain: Dictionary<DateTime, List<OptionBase>>
    {
        public double HistoricalVolatility { get; set; }
        public PriceList Prices { get; internal set; }
        public RiskFreeRates RiskFreeRates { get; internal set; }

        public void Add(OptionBase o)
        {
            if(this.ContainsKey(o.ExpirationDate) == false)
            {
                this.Add(o.ExpirationDate, new List<OptionBase>());
            }

            this[o.ExpirationDate].Add(o);
        }
    }
}
