using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Portfolios
{
    public class PortfolioBalance
    {
        private static readonly CultureInfo enUsCultureInfo = new CultureInfo("en-us");

        
        public int Id { get; set; }
        public int PortfolioId { get; set; }
        public DateTime Date { get; private set; }
        public string TransactionCode { get; private set; }
        public decimal NetCashBalance { get; private set; }

        public static PortfolioBalance From(string[] fields)
        {
            DateTime.TryParseExact(fields[0], "MM/dd/yyyy", enUsCultureInfo, DateTimeStyles.None, out DateTime result);
            PortfolioBalance b = new PortfolioBalance();
            b.Date = result;
            b.TransactionCode = fields[1];
            b.NetCashBalance = decimal.Parse(fields[8], enUsCultureInfo);
            return b;
        }
    }
}
