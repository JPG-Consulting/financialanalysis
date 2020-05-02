using FinancialAnalyst.Common.Entities.Assets;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Prices
{
    [Serializable]
    public class HistoricalPrice
    {
        public long Id { get; set; }

        /// <summary>
        /// Close price adjusted for splits (if data is from yahoo).
        /// </summary>
        public decimal Close { get; set; }

        public DateTime Date { get; set; }

        public ulong Volume { get; set; }

        [Required]
        public AssetBase Asset { get; set; }

        public decimal? Open { get; set; }

        public decimal? High { get; set; }

        public decimal? Low { get; set; }


        /// <summary>
        /// Adjusted close price adjusted for both dividends and splits (if data is from yahoo).
        /// </summary>
        public decimal? AdjustedClose { get; set; }

        /// <summary>
        /// Expected format:
        /// Date,Open,High,Low,Close,Adj Close,Volume
        /// Example:
        /// 1927-12-30,17.660000,17.660000,17.660000,17.660000,17.660000,0
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static HistoricalPrice From(string line)
        {
            string[] values = line.Split(',');
            HistoricalPrice p = new HistoricalPrice();
            p.Date = DateTime.ParseExact(values[0], "yyyy-MM-dd", null);
            //p.Open = 
            //p.High = 
            //p.Low = 
            p.Close = decimal.Parse(values[4]);
            p.AdjustedClose = decimal.Parse(values[5]);
            p.Volume = ulong.Parse(values[6]);
            return p;
        }
    }
}
