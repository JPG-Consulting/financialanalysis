using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Portfolios
{
    public class Transaction
    {
        private static readonly CultureInfo enUsCultureInfo = new CultureInfo("en-us");

        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public DateTime Date { get; set; }

        [JsonProperty]
        public string TransactionCode { get; set; }

        [JsonProperty]
        public string Description { get; set; }

        [JsonProperty]
        public int Quantity { get; set; }

        [JsonProperty]
        public string Symbol { get; set; }

        [JsonProperty]
        public decimal Price { get; set; }

        [JsonProperty]
        public decimal Commission { get; set; }

        [JsonProperty]
        [Required]
        public decimal Amount { get; set; }

        [JsonProperty]
        public decimal NetCashBalance { get; set; }

        [JsonProperty]
        public decimal RegFee { get; set; }

        [JsonIgnore]
        [Required]
        public int UserId { get; set; }

        [JsonIgnore]
        public int? PortfolioId { get; set; }

        [JsonProperty]
        [Required]
        public CashflowTypes CashflowType { get; private set; }

        public static Transaction From(int portfolioId, string[] fields)
        {
            if (fields == null || fields.Length < 10)
                return null;

            Transaction t = new Transaction();
            
            DateTime.TryParseExact(fields[0], "MM/dd/yyyy", enUsCultureInfo, DateTimeStyles.None, out DateTime result);
            t.Date = result;
            t.TransactionCode = fields[1];
            t.Description = fields[2];
            
            if (string.IsNullOrEmpty(fields[3]))
                t.Quantity = 0;
            else
                t.Quantity = int.Parse(fields[3]);

            if(string.IsNullOrEmpty(fields[4]) == false)
                t.Symbol = fields[4];

            if (string.IsNullOrEmpty(fields[5]))
                t.Price = 0;
            else
                t.Price = decimal.Parse(fields[5], enUsCultureInfo);

            if (string.IsNullOrEmpty(fields[6]))
                t.Commission = 0;
            else
                t.Commission = decimal.Parse(fields[6], enUsCultureInfo);
            
            t.Amount = decimal.Parse(fields[7], enUsCultureInfo);

            t.NetCashBalance = decimal.Parse(fields[8], enUsCultureInfo);
            
            if (string.IsNullOrEmpty(fields[9]))
                t.RegFee = 0;
            else
                t.RegFee = decimal.Parse(fields[9], enUsCultureInfo);


            if(t.Quantity == 0)
            {
                if (t.Amount >= 0)
                    t.CashflowType = CashflowTypes.IncomingCashflow;
                else
                    t.CashflowType = CashflowTypes.OutgoingCashflow;
            }
            else
            {
                if (t.Description.ToUpper().StartsWith(CashflowTypes.Bought.ToString().ToUpper()))
                    t.CashflowType = CashflowTypes.Bought;
                else if (t.Description.ToUpper().StartsWith(CashflowTypes.Sell.ToString().ToUpper()))
                    t.CashflowType = CashflowTypes.Sell;
                else
                    t.CashflowType = CashflowTypes.Unknown;
            }

            return t;
        }
    }
}
