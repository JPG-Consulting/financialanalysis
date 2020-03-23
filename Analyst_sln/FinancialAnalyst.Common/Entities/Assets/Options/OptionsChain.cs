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
