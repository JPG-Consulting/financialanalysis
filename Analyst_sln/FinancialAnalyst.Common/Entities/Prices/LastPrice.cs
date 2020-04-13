using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Prices
{
    [Serializable]
    public class LastPrice
    {
        public decimal Price { get; set; }
        public DateTime TimeStamp { get; set; }
        public int Volume { get; set; }
        public decimal PreviousClose { get; set; }
    }
}
