using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Assets
{
    public class OptionsChain:IEnumerable<KeyValuePair<DateTime, List<OptionBase>>>
    {
        private readonly Dictionary<DateTime, List<OptionBase>> options = new Dictionary<DateTime, List<OptionBase>>();

        public IEnumerator<KeyValuePair<DateTime, List<OptionBase>>> GetEnumerator()
        {
            return options.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return options.GetEnumerator();
        }

        public void Add(OptionBase o)
        {
            if(options.ContainsKey(o.ExpirationDate) == false)
            {
                options.Add(o.ExpirationDate, new List<OptionBase>());
            }

            options[o.ExpirationDate].Add(o);
        }
    }
}
