using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities
{
    public class Price
    {
        public DateTime Date { get; private set; }
        public double Close { get; private set; }
        public long Volume { get; private set; }

        /// <summary>
        /// Expected format:
        /// Date,Open,High,Low,Close,Adj Close,Volume
        /// Example:
        /// 1927-12-30,17.660000,17.660000,17.660000,17.660000,17.660000,0
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Price From(string line)
        {
            string[] values = line.Split(',');
            Price p = new Price();
            p.Date = DateTime.ParseExact(values[0], "yyyy-MM-dd", null);
            p.Close = double.Parse(values[4]);
            p.Volume = long.Parse(values[6]);
            return p;
        }
    }
}
