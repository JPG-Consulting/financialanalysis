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
        public decimal? Price_Last { get; set; }
        public DateTime? Price_Last_Time { get; set; }
        public decimal? Price_FiftyTwoWeekHigh { get; set; }
        public decimal? Price_FiftyTwoWeekLow { get; set; }
        public decimal? Beta { get; set; }
        public decimal? EarningsPerShare_ExcludingExtraItems_TTM { get; set; }
        public decimal? PriceEarnings_ExcludingExtraITems_TTM { get; set; }
        public decimal? PriceSales_Annual { get; set; }
        public decimal? PriceSales_TTM { get; set; }
        public decimal? PriceToCashFlow_PerShare_TTM { get; set; }
        public decimal? PriceBook_Annual { get; set; }
        public decimal? PriceBook_Quarterly { get; set; }
        public decimal? DividendYield { get; set; }
        public decimal? LongTermDebtToEquity_Annual { get; set; }
        public decimal? TotalDebtToEquity_Annual { get; set; }
        public decimal? LongTermDebtToEquity_Quarterly { get; set; }
        public decimal? TotalDebtToEquity_Quarterly { get; set; }
        public decimal? SharesOut { get; set; }
        public decimal? ROE_TTM { get; set; }
        public decimal? ROI_TTM { get; set; }
        public List<News> NewsList { get; set; }
        public List<Officer> Officers { get; set; }
    }
}
