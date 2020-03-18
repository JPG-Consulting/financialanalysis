using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Prices;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FinancialAnalyst.DataSources
{
    public class FileCacheManager : ICacheManager
    {
        private const string FOLDER = "HistoricalData";
        public bool TryGetFromCache(string ticker, Exchange? exchange, DateTime? from, DateTime? to, PriceInterval interval, out PriceList prices)
        {
            prices = new PriceList();
            try
            {
                string fileName = $"{ticker}";
                if (exchange.HasValue)
                    fileName += $"_{exchange}";

                if (from.HasValue)
                    fileName += $"_{from}";

                if (to.HasValue)
                    fileName += $"_{to}";

                fileName += $"_{interval.ToString()}.csv";

                string file = Path.Combine(FOLDER, fileName);
                
                if (File.Exists(file))
                {
                    string[] lines = File.ReadAllLines(file);
                    for (int i = 1; i < lines.Length; i++)
                    {
                        Price p = Price.From(lines[i]);
                        prices.Add(p);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception)
            {
                return false;
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }
    }
}
