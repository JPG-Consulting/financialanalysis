using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Accounting;
using FinancialAnalyst.Common.Entities.Prices;
using FinancialAnalyst.Common.Interfaces;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.DataSources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using FinancialAnalyst.Common.Entities.Users;

namespace FinancialAnalyst.DataSources.Reuters
{
    public class ReutersDataSource : IStockDataDataSource, IFinancialDataSource
	{
		public bool TryGetStockSummary(string ticker, Exchange? exchange, out Stock asset, out string errorMessage)
		{
			bool ok = false;
			string jsonResponse = "{}";
			string internalTicker = "";
			if (exchange.HasValue)
			{
				internalTicker = $"{ticker}{Translate(exchange.Value)}";
				ok = ReutersApiCaller.GetSummary(internalTicker, out jsonResponse, out errorMessage);
			}

			//If it fails the first try, it tries with all exchanges
			if (ok == false)
			{
				internalTicker = $"{ticker}";
				ok = ReutersApiCaller.GetSummary(internalTicker, out jsonResponse, out errorMessage);

				Exchange[] exchanges = (Exchange[])Enum.GetValues(typeof(Exchange));
				int i = 0;
				while (ok == false && i < exchanges.Length)
				{
					Exchange ex = exchanges[i];
					internalTicker = $"{ticker}{Translate(ex)}";
					ok = ReutersApiCaller.GetSummary(internalTicker, out jsonResponse, out errorMessage);
					i++;
				}
			}

			if (ok)
			{
                #region Data transformation
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
				s.TotalDebtToEquity_Quarterly = summary.market_data.total_debt_equity_quarterly;
				s.SharesOut = summary.market_data.shares_out;
				s.ROE_TTM = summary.market_data.roe_ttm;
				s.ROI_TTM = summary.market_data.roi_ttm;
				s.NewsList = new List<News>();
				if (summary.market_data.sig_devs != null)
				{
					foreach (var news in summary.market_data.sig_devs)
					{
						News n = new News();
						n.DateTime = news.last_update;
						n.Title = news.headline;
						n.Content = news.description;
						s.NewsList.Add(n);
					}
				}
				s.Officers = new List<Officer>();
				if (summary.market_data.officers != null)
				{
					foreach (var officer in summary.market_data.officers)
					{
						Officer o = new Officer();
						o.Name = officer.name;
						o.Rank = officer.rank;
						o.Title = officer.title;
						s.Officers.Add(o);
					}
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
				#endregion
				errorMessage = $"Ticker {internalTicker} founded.";
				asset = s;
				return true;
			}
			else
			{
				if (exchange.HasValue)
					errorMessage = $"It can't find asset data for ticker {ticker} in exchange {exchange.Value.ToString()}";
				else
					errorMessage = $"It can't find asset data for ticker {ticker} in all markets available";
				asset = null;
				return false;
			}
		}

		public bool TryGetFinancialData(string ticker, Exchange? exchange, out FinancialStatements financialData, out string errorMessage)
		{
			//Example
			//https://www.reuters.com/companies/api/getFetchCompanyFinancials/AAPL.OQ

			bool ok = false;
			string jsonResponse = "{}";
			string internalTicker = "";

			internalTicker = $"{ticker}{Translate(exchange.Value)}";
			ok = ReutersApiCaller.GetFinancialData(internalTicker, out jsonResponse, out errorMessage);

			//If it fails the first try, it tries with all exchanges
			if (ok == false)
			{
				internalTicker = $"{ticker}";
				ok = ReutersApiCaller.GetFinancialData(internalTicker, out jsonResponse, out errorMessage);

				Exchange[] exchanges = (Exchange[])Enum.GetValues(typeof(Exchange));
				int i = 0;
				while (ok == false && i < exchanges.Length)
				{
					Exchange ex = exchanges[i];
					internalTicker = $"{ticker}{Translate(ex)}";
					ok = ReutersApiCaller.GetSummary(internalTicker, out jsonResponse, out errorMessage);
					i++;
				}
			}

			if (ok)
			{
				dynamic rawdata = JsonConvert.DeserializeObject(jsonResponse);
				financialData = new FinancialStatements()
				{
					IncomeStatement = new AccountingItem() { IsAnnual = true, Childs = new List<AccountingItem>(), },
					BalanceSheet = new AccountingItem() { IsAnnual = true, Childs = new List<AccountingItem>(), },
					CashFlowStatement = new AccountingItem() { IsAnnual = true, Childs = new List<AccountingItem>(), },
						
				};

				AddValues(financialData.IncomeStatement.Childs, rawdata.market_data.financial_statements.income.annual);
				AddValues(financialData.BalanceSheet.Childs, rawdata.market_data.financial_statements.balance_sheet.annual);
				AddValues(financialData.CashFlowStatement.Childs, rawdata.market_data.financial_statements.cash_flow.annual);

				return true;
			}
			else
			{
				if (exchange.HasValue)
					errorMessage = $"It can't find financial data for ticker {ticker} in exchange {exchange.Value.ToString()}.";
				else
					errorMessage = $"It can't find financial data for ticker {ticker} in all markets available.";
				financialData = null;
				return false;
			}
		}

		private void AddValues(List<AccountingItem> items, dynamic collection)
		{
			foreach (var item in collection)
			{
				AccountingItem accountingItem = new AccountingItem()
				{
					Name = item.Name,
					ValuesPerPeriod = new Dictionary<DateTime, double>(),
				};

				foreach (JToken datevalue in item.Value)
				{
					DateTime date = datevalue.Value<DateTime>("date");
					double value = datevalue.Value<double>("value");
					accountingItem.ValuesPerPeriod.Add(date, value);
				}
				items.Add(accountingItem);
			}
		}

		private string Translate(Exchange exchange)
		{
			switch (exchange)
			{
				case Exchange.NASDAQ:
					return ".OQ";
				case Exchange.NYSE:
					return ".N";
				default:
					throw new NotImplementedException($"There is no translation for exchange {exchange.ToString()}");
			}
		}

		/* Exchanges for JPMorgan
		<table width="100%" cellspacing="0" cellpadding="1" class="search-table-data">
		<tr>
			<th>Company</th>
			<th>Symbol</th>
			<th>Exchange</th>
		</tr>
			<tr class="stripe" onclick="parent.location='/companies/JPM.A'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM.A</td>
					<td>American Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JPM.N'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM.N</td>
					<td>New York Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JPM.BA'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM.BA</td>
					<td>Buenos Aires Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JPM.BE'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM.BE</td>
					<td>Berlin Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JPM.BN'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM.BN</td>
					<td>Berne Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JPM.C'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM.C</td>
					<td>Cincinatti Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JPM.D'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM.D</td>
					<td>Dusseldorf Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JPM.DE'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM.DE</td>
					<td>Xetra</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JPM.DF'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM.DF</td>
					<td>NASD Alternative Display Facility for NYSE/AMEX Issues</td>
				</tr>

			<tr onclick="parent.location='/companies/JPM.EI'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM.EI</td>
					<td>Investors Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JPM.F'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM.F</td>
					<td>Frankfurt Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JPM.H'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM.H</td>
					<td>Hamburg Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JPM.HA'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM.HA</td>
					<td>Hanover Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JPM.MU'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM.MU</td>
					<td>Munich Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JPM.MW'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM.MW</td>
					<td>Midwest Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JPM.MX'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM.MX</td>
					<td>Mexico Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JPM.P'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM.P</td>
					<td>NYSE Arca</td>
				</tr>

			<tr onclick="parent.location='/companies/JPM.PH'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM.PH</td>
					<td>Philadelphia Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JPM.S'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM.S</td>
					<td>Swiss Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JPM.SN'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM.SN</td>
					<td>Santiago Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JPM.VI'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM.VI</td>
					<td>Vienna Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JPM.VIf'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM.VIf</td>
					<td>Vienna Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JPM.Z'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM.Z</td>
					<td>BATS Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JPM'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM</td>
					<td>New York Consolidated</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/AMJ'">
					<td>JPMorgan Chase & Co</td>
					<td>AMJ</td>
					<td>Consolidated Issue Listed By NYSE Arca</td>
				</tr>

			<tr onclick="parent.location='/companies/AMJ.DF'">
					<td>JPMorgan Chase & Co</td>
					<td>AMJ.DF</td>
					<td>NASD Alternative Display Facility for NYSE/AMEX Issues</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/AMJ.MW'">
					<td>JPMorgan Chase & Co</td>
					<td>AMJ.MW</td>
					<td>Midwest Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/AMJ.P'">
					<td>JPMorgan Chase & Co</td>
					<td>AMJ.P</td>
					<td>NYSE Arca</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/AMJ.PH'">
					<td>JPMorgan Chase & Co</td>
					<td>AMJ.PH</td>
					<td>Philadelphia Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/AMJ.Z'">
					<td>JPMorgan Chase & Co</td>
					<td>AMJ.Z</td>
					<td>BATS Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/AMJiv.P'">
					<td>JPMorgan Chase & Co</td>
					<td>AMJiv.P</td>
					<td>NYSE Arca</td>
				</tr>

			<tr onclick="parent.location='/companies/AMJso.P'">
					<td>JPMorgan Chase & Co</td>
					<td>AMJso.P</td>
					<td>NYSE Arca</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JPMB.A'">
					<td>JPMorgan USD Emerging Market Sovereign Bond ETF</td>
					<td>JPMB.A</td>
					<td>American Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JPMB.N'">
					<td>JPMorgan USD Emerging Market Sovereign Bond ETF</td>
					<td>JPMB.N</td>
					<td>New York Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JPME.A'">
					<td>JPMorgan Diversified Return US Mid Cap Equity ETF</td>
					<td>JPME.A</td>
					<td>American Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JPME.N'">
					<td>JPMorgan Diversified Return US Mid Cap Equity ETF</td>
					<td>JPME.N</td>
					<td>New York Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JPMF.A'">
					<td>JPM MGD FT STRGY</td>
					<td>JPMF.A</td>
					<td>American Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JPMF.N'">
					<td>JPMorgan Managed Futures Strategy ETF</td>
					<td>JPMF.N</td>
					<td>New York Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JPMV.A'">
					<td>iShares Edge MSCI Min Vol Japan ETF</td>
					<td>JPMV.A</td>
					<td>American Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JPMV.N'">
					<td>iShares Edge MSCI Min Vol Japan ETF</td>
					<td>JPMV.N</td>
					<td>New York Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JPM_pc.A'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM_pc.A</td>
					<td>American Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JPM_pc.N'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM_pc.N</td>
					<td>New York Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JPM_pd.A'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM_pd.A</td>
					<td>American Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JPM_pd.N'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM_pd.N</td>
					<td>New York Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JPM_pf.A'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM_pf.A</td>
					<td>American Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JPM_pf.N'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM_pf.N</td>
					<td>New York Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JPM_pg.A'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM_pg.A</td>
					<td>American Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JPM_pg.N'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM_pg.N</td>
					<td>New York Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JPM_ph.A'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM_ph.A</td>
					<td>American Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JPM_ph.N'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM_ph.N</td>
					<td>New York Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JPM_pj.A'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM_pj.A</td>
					<td>American Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JPM_pj.N'">
					<td>JPMorgan Chase & Co</td>
					<td>JPM_pj.N</td>
					<td>New York Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JPM05.AX'">
					<td>JPMorgan Global Research Enhanced Index Equity</td>
					<td>JPM05.AX</td>
					<td>Australia Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JPM06.AX'">
					<td>JPMorgan Global Research Enhanced Index Equity H</td>
					<td>JPM06.AX</td>
					<td>Australia Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JAI.L'">
					<td>JPMorgan Asian Investment Trust PLC</td>
					<td>JAI.L</td>
					<td>London Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JAM.L'">
					<td>JPMorgan American Investment Trust PLC</td>
					<td>JAM.L</td>
					<td>London Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JFAIF.PK'">
					<td>JPMorgan American Investment Trust PLC</td>
					<td>JFAIF.PK</td>
					<td>OTC Markets Group - US Other OTC and Grey Market</td>
				</tr>

			<tr onclick="parent.location='/companies/JCGI.L'">
					<td>JPMorgan China Growth & Income PLC</td>
					<td>JCGI.L</td>
					<td>London Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JCH.L'">
					<td>JPmorgan Claverhouse Investment Trust PLC</td>
					<td>JCH.L</td>
					<td>London Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JEMI.L'">
					<td>JPmorgan Global Emerging Markets Income Trust PLC</td>
					<td>JEMI.L</td>
					<td>London Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JESC.L'">
					<td>JPMorgan European Smaller Companies Trust PLC</td>
					<td>JESC.L</td>
					<td>London Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JETi.L'">
					<td>Jpmorgan European Investment Trust PLC</td>
					<td>JETi.L</td>
					<td>London Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JETx.L'">
					<td>Jpmorgan European Investment Trust PLC</td>
					<td>JETx.L</td>
					<td>London Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JFJ.L'">
					<td>JPMorgan Japanese Investment Trust PLC</td>
					<td>JFJ.L</td>
					<td>London Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JII.L'">
					<td>JPMorgan Indian Investment Trust PLC</td>
					<td>JII.L</td>
					<td>London Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JPIVF.PK'">
					<td>JPMorgan Indian Investment Trust PLC</td>
					<td>JPIVF.PK</td>
					<td>OTC Markets Group - US Other OTC and Grey Market</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JMF.L'">
					<td>JPMorgan Mid Cap Investment Trust PLC</td>
					<td>JMF.L</td>
					<td>London Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JMG.L'">
					<td>JPmorgan Emerging Markets Investment Trust PLC</td>
					<td>JMG.L</td>
					<td>London Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JMI.L'">
					<td>JPmorgan Smaller Companies Investment Trust PLC</td>
					<td>JMI.L</td>
					<td>London Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JPB.L'">
					<td>JPMorgan Brazil Investment Trust PLC</td>
					<td>JPB.L</td>
					<td>London Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JPEi.L'">
					<td>Jpmorgan Elect PLC</td>
					<td>JPEi.L</td>
					<td>London Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JPE.L'">
					<td>Jpmorgan Elect PLC</td>
					<td>JPE.L</td>
					<td>London Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JPEx.L'">
					<td>Jpmorgan Elect PLC</td>
					<td>JPEx.L</td>
					<td>London Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JPGI.L'">
					<td>Jpmorgan Global Growth & Income PLC</td>
					<td>JPGI.L</td>
					<td>London Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JPG.NZ'">
					<td>Jpmorgan Global Growth & Income PLC</td>
					<td>JPG.NZ</td>
					<td>New Zealand Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JPM2.L'">
					<td>Leverage Shares Public Ltd Co</td>
					<td>JPM2.L</td>
					<td>London Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JPM2l.TRE'">
					<td>Leverage Shares Public Ltd Co</td>
					<td>JPM2l.TRE</td>
					<td>London Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JPM2l1.TRE'">
					<td>Leverage Shares Public Ltd Co</td>
					<td>JPM2l1.TRE</td>
					<td>London Stock Exchange</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JPM2l2.TRE'">
					<td>Leverage Shares Public Ltd Co</td>
					<td>JPM2l2.TRE</td>
					<td>London Stock Exchange</td>
				</tr>

			<tr onclick="parent.location='/companies/JPGEec.P'">
					<td>JPMorgan Diversified Return Global Equity ETF</td>
					<td>JPGEec.P</td>
					<td>NYSE Arca</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JPGEiv.P'">
					<td>JPMorgan Diversified Return Global Equity ETF</td>
					<td>JPGEiv.P</td>
					<td>NYSE Arca</td>
				</tr>

			<tr onclick="parent.location='/companies/JPGEnv.P'">
					<td>JPMorgan Diversified Return Global Equity ETF</td>
					<td>JPGEnv.P</td>
					<td>NYSE Arca</td>
				</tr>

			<tr class="stripe" onclick="parent.location='/companies/JPGEso.P'">
					<td>JPMorgan Diversified Return Global Equity ETF</td>
					<td>JPGEso.P</td>
					<td>NYSE Arca</td>
				</tr>

			<tr onclick="parent.location='/companies/JPGEtc.P'">
					<td>JPMorgan Diversified Return Global Equity ETF</td>
					<td>JPGEtc.P</td>
					<td>NYSE Arca</td>
				</tr>

			</table> 
		*/

		/* Exchanges for AAPLE
		<table width="100%" cellspacing="0" cellpadding="1" class="search-table-data">
			<tr>
				<th>Company</th>
				<th>Symbol</th>
				<th>Exchange</th>
			</tr>
			<tr class="stripe" onclick="parent.location='/companies/AAPL.O'">
				<td>Apple Inc</td>
				<td>AAPL.O</td>
				<td>Nasdaq</td>
			</tr>

			<tr onclick="parent.location='/companies/AAPL.OQ'">
				<td>Apple Inc</td>
				<td>AAPL.OQ</td>
				<td>NASDAQ Stock Exchange Global Select Market</td>
			</tr>

			<tr class="stripe" onclick="parent.location='/companies/AAPL.BA'">
				<td>Apple Inc</td>
				<td>AAPL.BA</td>
				<td>Buenos Aires Stock Exchange</td>
			</tr>

			<tr onclick="parent.location='/companies/AAPL.BE'">
				<td>Apple Inc</td>
				<td>AAPL.BE</td>
				<td>Berlin Stock Exchange</td>
			</tr>

			<tr class="stripe" onclick="parent.location='/companies/AAPL.BN'">
				<td>Apple Inc</td>
				<td>AAPL.BN</td>
				<td>Berne Stock Exchange</td>
			</tr>

			<tr onclick="parent.location='/companies/AAPL.C'">
				<td>Apple Inc</td>
				<td>AAPL.C</td>
				<td>Cincinatti Stock Exchange</td>
			</tr>

			<tr class="stripe" onclick="parent.location='/companies/AAPL.D'">
				<td>Apple Inc</td>
				<td>AAPL.D</td>
				<td>Dusseldorf Stock Exchange</td>
			</tr>

			<tr onclick="parent.location='/companies/AAPL.DE'">
				<td>Apple Inc</td>
				<td>AAPL.DE</td>
				<td>Xetra</td>
			</tr>

			<tr class="stripe" onclick="parent.location='/companies/AAPL.DF'">
				<td>Apple Inc</td>
				<td>AAPL.DF</td>
				<td>NASD ADF</td>
			</tr>

			<tr onclick="parent.location='/companies/AAPL.F'">
				<td>Apple Inc</td>
				<td>AAPL.F</td>
				<td>Frankfurt Stock Exchange</td>
			</tr>

			<tr class="stripe" onclick="parent.location='/companies/AAPL.H'">
				<td>Apple Inc</td>
				<td>AAPL.H</td>
				<td>Hamburg Stock Exchange</td>
			</tr>

			<tr onclick="parent.location='/companies/AAPL.HA'">
				<td>Apple Inc</td>
				<td>AAPL.HA</td>
				<td>Hanover Stock Exchange</td>
			</tr>

			<tr class="stripe" onclick="parent.location='/companies/AAPL.LM'">
				<td>Apple Inc</td>
				<td>AAPL.LM</td>
				<td>Lima Stock Exchange</td>
			</tr>

			<tr onclick="parent.location='/companies/AAPL.MU'">
				<td>Apple Inc</td>
				<td>AAPL.MU</td>
				<td>Munich Stock Exchange</td>
			</tr>

			<tr class="stripe" onclick="parent.location='/companies/AAPL.MW'">
				<td>Apple Inc</td>
				<td>AAPL.MW</td>
				<td>Midwest Stock Exchange</td>
			</tr>

			<tr onclick="parent.location='/companies/AAPL.MX'">
				<td>Apple Inc</td>
				<td>AAPL.MX</td>
				<td>Mexico Stock Exchange</td>
			</tr>

			<tr class="stripe" onclick="parent.location='/companies/AAPL.P'">
				<td>Apple Inc</td>
				<td>AAPL.P</td>
				<td>NYSE Arca</td>
			</tr>

			<tr onclick="parent.location='/companies/AAPL.PH'">
				<td>Apple Inc</td>
				<td>AAPL.PH</td>
				<td>Philadelphia Stock Exchange</td>
			</tr>

			<tr class="stripe" onclick="parent.location='/companies/AAPL.S'">
				<td>Apple Inc</td>
				<td>AAPL.S</td>
				<td>Swiss Exchange</td>
			</tr>

			<tr onclick="parent.location='/companies/AAPL.SN'">
				<td>Apple Inc</td>
				<td>AAPL.SN</td>
				<td>Santiago Stock Exchange</td>
			</tr>

			<tr class="stripe" onclick="parent.location='/companies/AAPL.VI'">
				<td>Apple Inc</td>
				<td>AAPL.VI</td>
				<td>Vienna Stock Exchange</td>
			</tr>

			<tr onclick="parent.location='/companies/AAPL.VIf'">
				<td>Apple Inc</td>
				<td>AAPL.VIf</td>
				<td>Vienna Stock Exchange</td>
			</tr>

			<tr class="stripe" onclick="parent.location='/companies/AAPL34.SA'">
				<td>Apple Inc</td>
				<td>AAPL34.SA</td>
				<td>Sao Paulo Stock Exchange</td>
			</tr>

			<tr onclick="parent.location='/companies/AAPL34F.SA'">
				<td>Apple Inc</td>
				<td>AAPL34F.SA</td>
				<td>Sao Paulo Stock Exchange</td>
			</tr>

			<tr class="stripe" onclick="parent.location='/companies/AAPLc.BA'">
				<td>Apple Inc</td>
				<td>AAPLc.BA</td>
				<td>Buenos Aires Stock Exchange</td>
			</tr>

			<tr onclick="parent.location='/companies/AAPLd.BA'">
				<td>Apple Inc</td>
				<td>AAPLd.BA</td>
				<td>Buenos Aires Stock Exchange</td>
			</tr>

			<tr class="stripe" onclick="parent.location='/companies/AAPLE.MI'">
				<td>Apple Inc</td>
				<td>AAPLE.MI</td>
				<td>Milan Stock Exchange</td>
			</tr>

			<tr onclick="parent.location='/companies/AAPLEtah.MI'">
				<td>Apple Inc</td>
				<td>AAPLEtah.MI</td>
				<td>Milan Stock Exchange</td>
			</tr>

			<tr class="stripe" onclick="parent.location='/companies/AAPLEUR.S'">
				<td>Apple Inc</td>
				<td>AAPLEUR.S</td>
				<td>Swiss Exchange</td>
			</tr>

			<tr onclick="parent.location='/companies/AAPLh.BA'">
				<td>Apple Inc</td>
				<td>AAPLh.BA</td>
				<td>Buenos Aires Stock Exchange</td>
			</tr>

			<tr class="stripe" onclick="parent.location='/companies/AAPLm.BA'">
				<td>Apple Inc</td>
				<td>AAPLm.BA</td>
				<td>Buenos Aires Stock Exchange</td>
			</tr>

			<tr onclick="parent.location='/companies/AAPLUSD.S'">
				<td>Apple Inc</td>
				<td>AAPLUSD.S</td>
				<td>Swiss Exchange</td>
			</tr>

		</table>

		 */

	}
}
