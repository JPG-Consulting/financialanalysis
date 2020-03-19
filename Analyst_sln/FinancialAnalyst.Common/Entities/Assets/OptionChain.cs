using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Assets
{
    public class OptionChain
    {
        public readonly Dictionary<DateTime, List<Option>> options = new Dictionary<DateTime, List<Option>>();
    }
}
