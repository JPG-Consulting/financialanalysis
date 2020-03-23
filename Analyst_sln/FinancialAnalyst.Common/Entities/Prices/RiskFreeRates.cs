using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Prices
{
    public class RiskFreeRates
    {
        public DateTime Date { get; set; }
        public double OneMonth { get; set; }
        public double TwoMonths { get; set; }
        public double ThreeMonths { get; set; }
        public double SixMonths { get; set; }
        public double OneYear { get; set; }
        public double TwoYears { get; set; }
        public double ThreeYears { get; set; }
        public double FiveYears { get; set; }
        public double SevenYears { get; set; }
        public double TenYears { get; set; }
        public double TwentyYears { get; set; }
        public double ThirtyYears { get; set; }
    }
}
