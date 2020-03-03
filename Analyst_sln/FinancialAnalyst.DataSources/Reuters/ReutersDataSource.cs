using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.DataSources.Reuters
{
    public class ReutersDataSource : IDataSource
    {
        public bool TryGetAssetData(string ticker, Market market, out AssetBase asset, out string errorMessage)
        {
            string internalTicker = $"{ticker}{Translate(market)}";
            bool ok = ReutersWebAPI.GetSummary(internalTicker,out string jsonResponse, out errorMessage);
            if(ok)
            {
                dynamic summary = JsonConvert.DeserializeObject(jsonResponse);
                Stock s = new Stock();
                s.CompanyName = summary.market_data.company_name;
                s.Description = summary.market_data.about;
                s.WebSite = summary.market_data.website;
                s.WebSite_Source = $"https://www.reuters.com/companies/{internalTicker}";
                s.Sector = summary.market_data.sector;
                s.Industry = summary.market_data.industry;
                s.Country = summary.country;
                s.Price_Last = summary.market_data.last;
                s.Price_Last_Time = summary.market_data.last_time;
                s.Price_FiftyTwoWeekHigh = summary.market_data.fiftytwo_wk_high;
                s.Price_FiftyTwoWeekLow = summary.market_data.fiftytwo_wk_low;
                s.Beta = summary.market_data.beta;
                s.EarningsPerShare_ExcludingExtraItems_TTM = summary.market_data.eps_excl_extra_ttm;
                s.PriceEarnings_ExcludingExtraITems_TTM = summary.market_data.pe_excl_extra_ttm;
                s.PriceSales_Annual = summary.market_data.ps_annual;
                s.PriceSales_TTM = summary.market_data.ps_ttm;
                s.PriceToCashFlow_PerShare_TTM = summary.market_data.pcf_share_ttm;
                s.PriceBook_Annual = summary.market_data.pb_annual;
                s.PriceBook_Quarterly = summary.market_data.pb_quarterly;
                s.DividendYield = summary.market_data.dividend_yield_indicated_annual;
                s.LongTermDebtToEquity_Annual = summary.market_data.lt_debt_equity_annual;
                s.TotalDebtToEquity_Annual = summary.market_data.total_debt_equity_annual;
                s.LongTermDebtToEquity_Quarterly = summary.market_data.lt_debt_equity_quarterly;
                s.TotalDebtToEquity_Quarterly= summary.market_data.total_debt_equity_quarterly;
                s.SharesOut = summary.market_data.shares_out;
                s.ROE_TTM = summary.market_data.roe_ttm;
                s.ROI_TTM = summary.market_data.roi_ttm;
                s.NewsList = new List<News>();
                foreach(var news in summary.market_data.sig_devs)
                {
                    News n = new News();
                    n.DateTime = news.last_update;
                    n.Title = news.headline;
                    n.Content = news.description;
                    s.NewsList.Add(n);
                }
                s.Officers = new List<Officer>();
                foreach (var officer in summary.market_data.officers)
                {
                    Officer o = new Officer();
                    o.Name = officer.name;
                    o.Rank = officer.rank;
                    o.Title = officer.title;
                    s.Officers.Add(o);
                }

                /*
                s.EarningsPerYear = summary.market_data.eps_per_year...;
                "eps_per_year": {
                  "currency": "USD",
                  "data": [
                    {
                      "fiscal_year": 2017,
                      "value": 1.91,
                      "estimate": false
                    },
                    {
                      "fiscal_year": 2018,
                      "value": 2.08,
                      "estimate": false
                    },
                    {
                      "fiscal_year": 2019,
                      "value": 2.11,
                      "estimate": false
                    },
                    {
                      "fiscal_year": 2020,
                      "value": 2.256,
                      "estimate": true
                    }
                  ]
                },
                */

                /*
                s.RevenuePerYear= = summary.market_data.revenue_per_year...;
                "revenue_per_year": {
                  "currency": "USD",
                  "data": [
                    {
                      "fiscal_year": 2017,
                      "value": 35416,
                      "estimate": false
                    },
                    {
                      "fiscal_year": 2018,
                      "value": 31847,
                      "estimate": false
                    },
                    {
                      "fiscal_year": 2019,
                      "value": 37280,
                      "estimate": false
                    },
                    {
                      "fiscal_year": 2020,
                      "value": 39016.251125,
                      "estimate": true
                    }
                  ]
                },
                */
                asset = s;
                return true;
            }
            else
            {
                asset = null;
                return false;
            }
        }

        private string Translate(Market market)
        {
            switch(market)
            {
                case Market.NASDAQ:
                    return ".OQ";
                case Market.NYSE:
                    return "N";
                default:
                    throw new NotImplementedException($"There is no translation for market {market.ToString()}");
            }
        }
    }
}
