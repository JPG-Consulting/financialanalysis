using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Assets
{
    [Serializable]
    public class Stock : AssetBase
    {
        public override AssetType AssetType { get { return AssetType.Stock; } }

        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string WebSite { get; set; }
        public string WebSite_Source { get; set; }
        public string Sector { get; set; }
        public string Industry { get; set; }
        public string Country { get; set; }
        public double? Price_Last { get; set; }
        public DateTime? Price_Last_Time { get; set; }
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
        public double? Volatility { get; set; }

        public OptionsChain OptionsChain { get; set; }
    }
}
