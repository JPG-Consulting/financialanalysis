using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.DataSources.FinancialDataSources.Yahoo
{
    //Valid intervals: [1m, 2m, 5m, 15m, 30m, 60m, 90m, 1h, 1d, 5d, 1wk, 1mo, 3mo]
    public class YahooChartInterval
    {
        private readonly string value;
        private YahooChartInterval(string value)
        {
            this.value = value;
        }

        public static readonly YahooChartInterval OneMinute = new YahooChartInterval("1m");
        public static readonly YahooChartInterval TwoMinutes = new YahooChartInterval("2m");
        public static readonly YahooChartInterval FiveMinutes = new YahooChartInterval("5m");
        public static readonly YahooChartInterval FifteenMinutes = new YahooChartInterval("15m");
        public static readonly YahooChartInterval ThirtyMinutes = new YahooChartInterval("30m");
        public static readonly YahooChartInterval Sixty = new YahooChartInterval("60m");
        public static readonly YahooChartInterval NintyMinutes = new YahooChartInterval("90m");
        public static readonly YahooChartInterval OneHour = new YahooChartInterval("1h");
        public static readonly YahooChartInterval OneDay = new YahooChartInterval("1d");
        public static readonly YahooChartInterval FiveDays = new YahooChartInterval("5d");
        public static readonly YahooChartInterval OneWeek = new YahooChartInterval("1wk");
        public static readonly YahooChartInterval OneMonth = new YahooChartInterval("1mo");
        public static readonly YahooChartInterval ThreeMonths = new YahooChartInterval("3mo");

        public override string ToString()
        {
            return value;
        }
    }
}
