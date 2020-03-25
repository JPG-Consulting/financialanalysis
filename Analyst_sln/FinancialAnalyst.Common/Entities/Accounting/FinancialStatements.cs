using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Accounting
{
    /// <summary>
    /// https://www.investopedia.com/terms/f/financial-statements.asp
    /// 
    /// https://www.investopedia.com/articles/basics/06/financialreporting.asp
    /// </summary>
    public class FinancialStatements
    {
        public AccountingItem BalanceSheet { get; set; }
        public AccountingItem IncomeStatement { get; set; }
        public AccountingItem CashFlowStatement { get; set; }
    }

    public class AccountingItem
    {
        public int Order { get; set; }
        public string Name { get; set; }
        public Dictionary<DateTime, double> ValuesPerPeriod { get; set; }
        public List<AccountingItem> Childs { get; set; }
        public bool? IsAnnual { get; set; }
    }

    public struct AccountKey
    {
        //https://stackoverflow.com/questions/1171812/multi-key-dictionary-in-c
        //https://stackoverflow.com/questions/689940/hashtable-with-multidimensional-key-in-c-sharp

        public readonly string AccountName;
        public readonly int Year;

        public AccountKey(string accountName, int year)
        {
            AccountName = accountName;
            Year = year;
        }
    }
}
