using FinancialAnalyst.Common.Entities.Accounting;
using FinancialAnalyst.Common.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Assets
{
    [Serializable]
    public class Stock : AssetBase
    {
        public override AssetClass AssetClass 
        { 
            get { return AssetClass.Stock; }
            protected set { }
        }
        public string CompanyName { get; set; }
        public string Description { get; set; }

        public string WebSite { get; set; }

        public double? Volatility { get; set; }

        [NotMapped]//TODO: Fix this mapping!!!
        public OptionsChain OptionsChain { get; set; }

        [NotMapped]//TODO: Fix this mapping!!!
        public FinancialStatements FinancialStatements { get; set; }

        [NotMapped]//TODO: Fix this mapping!!!
        public StockRelatedData StockRelatedData { get; set; }

        public Stock(string ticker) : base(ticker)
        {

        }
    }

    public class StockRelatedData
    { 
        
        public string WebSite_DataSource { get; set; }
        public string Sector { get; set; }
        public string Industry { get; set; }
        public string Country { get; set; }
        
        public double? Price_FiftyTwoWeekHigh { get; set; }
        public double? Price_FiftyTwoWeekLow { get; set; }
        public double? Beta { get; set; }
        public double? EarningsPerShare_ExcludingExtraItems_TTM { get; set; }
        public double? PriceEarnings_ExcludingExtraITems_TTM { get; set; }
        public double? PriceSales_Annual { get; set; }
        public double? PriceSales_TTM { get; set; }
        public double? PriceToCashFlow_PerShare_TTM { get; set; }
        public double? PriceBook_Annual { get; set; }
        public double? PriceBook_Quarterly { get; set; }
        public double? DividendYield { get; set; }
        public double? LongTermDebtToEquity_Annual { get; set; }
        public double? TotalDebtToEquity_Annual { get; set; }
        public double? LongTermDebtToEquity_Quarterly { get; set; }
        public double? TotalDebtToEquity_Quarterly { get; set; }
        public double? SharesOut { get; set; }
        public double? ROE_TTM { get; set; }
        public double? ROI_TTM { get; set; }
        public List<News> NewsList { get; set; }
        public List<Officer> Officers { get; set; }
        

#pragma warning restore CA2235 // Mark all non-serializable fields
    }
}
