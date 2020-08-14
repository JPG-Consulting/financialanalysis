using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Markets;
using FinancialAnalyst.Common.Entities.Prices;
using FinancialAnalyst.Common.Interfaces;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.DataSources;
using FinancialAnalyst.DataSources.FinancialDataSources.Yahoo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace FinancialAnalyst.DataSources.FinancialDataSources.Yahoo
{
    public class YahooDataSource : IStockDataDataSource, IPricesDataSource, IAssetTypeDataSource, IStatisticsDataSource, IIndexesDataSource, IOptionChainDataSource
    {
        private static readonly DateTime FIRST_DATE = new DateTime(1927, 12, 30, 0, 0, 0);
        internal static readonly DateTime DATE_1970 = new DateTime(1970, 1, 1, 0, 0, 0);

        public bool TryGetStockSummary(string ticker, Exchange? exchange, AssetClass assetClass, out Stock asset, out string errorMessage)
        {
            //https://query1.finance.yahoo.com/v10/finance/quoteSummary/AAPL?formatted=true&crumb=.hu5lEQsLEy&lang=en-US&region=US&modules=assetProfile%2CsecFilings&corsDomain=finance.yahoo.com
            //https://query1.finance.yahoo.com/v10/finance/quoteSummary/AAPL?formatted=true&lang=en-US&region=US&modules=assetProfile%2CsecFilings&corsDomain=finance.yahoo.com
            throw new NotImplementedException();
        }

        public bool TryGetAssetType(string ticker, out AssetClass assetType)
        {
            //Example 1
            //https://query2.finance.yahoo.com/v7/finance/quote?formatted=true&lang=en-US&region=US&symbols=AAPL
            /*
            {
              "quoteResponse": {
                "result": [
                  {
                    ...
                    "symbol": "AAPL",
                    "quoteType": "EQUITY",
                    "longName": "Apple Inc."
                    ...
                  }
                ],
                "error": null
              }
            }
            */

            //Example 2
            //https://query2.finance.yahoo.com/v7/finance/quote?formatted=true&lang=en-US&region=US&symbols=TQQQ
            /*
            {
              "quoteResponse": {
                "result": [
                  {
                    "symbol": "TQQQ",
                    "shortName": "ProShares UltraPro QQQ",
                    "quoteType": "ETF",
                    "longName": "ProShares UltraPro QQQ"
                  }
                ],
                "error": null
              }
            }
            */

            bool ok = YahooApiCaller.GetQuoteData(ticker, out HttpStatusCode statusCode, out YahooQuoteResponse yahooResponse, out string jsonResponse, out string message);
            if(ok)
            {
                var result = yahooResponse.quoteResponse.result.Where(r => r.symbol == ticker).SingleOrDefault();
                if(result == null)
                {
                    assetType = AssetClass.Unknown;
                    return false;
                }

                if(result.quoteType == "ETF")
                {
                    assetType = AssetClass.ETF;
                    return true;
                }
                else if (result.quoteType == "EQUITY")
                {
                    assetType = AssetClass.Stock;
                    return true;
                }
                else
                {
                    assetType = AssetClass.Unknown;
                    return false;
                }
            }
            else
            {
                assetType = AssetClass.Unknown;
                return false;
            }


        }

        public bool TryGetLastPrice(string ticker, Exchange? exchange, AssetClass assetType, out HistoricalPrice lastPrice, out string message)
        {
            //Last two months???
            //https://query1.finance.yahoo.com/v8/finance/chart/AAPL?region=US&lang=en-US&includePrePost=false&interval=2m&range=1d&corsDomain=finance.yahoo.com&.tsrc=finance

            //Daily, last day
            //https://query1.finance.yahoo.com/v8/finance/chart/AAPL?lang=en-US&includePrePost=false&interval=1d&range=1d

            //Daily, last 5 days
            //https://query1.finance.yahoo.com/v8/finance/chart/AAPL?lang=en-US&includePrePost=false&interval=1d&range=5d

            int days = 1;
            YahooChartInterval interval = YahooChartInterval.OneHour;
            bool ok = YahooApiCaller.GetDailyPrices(ticker, interval,  days, out HttpStatusCode statusCode, out YahooChartResponse yahooResponse, out string jsonResponse, out message);

            if (ok && statusCode == HttpStatusCode.OK && yahooResponse != null && yahooResponse.Chart != null && yahooResponse.Chart.Result != null && yahooResponse.Chart.Result.Length > 0)
            {
                var result = yahooResponse.Chart.Result.Last();
                int length = result.TimeStampsAsLong.Length;
                decimal? close = null;
                int i = length - 1;
                while(close ==null && i > 0)
                {
                    if (result.Indicators.Quote[0].Close[i].HasValue)
                        close = result.Indicators.Quote[0].Close[i].Value;
                    else
                        i--;
                }
                int index = i;
                long seconds = result.TimeStampsAsLong[index];
                DateTime dt = DATE_1970.AddSeconds(seconds);
                decimal price = result.Indicators.Quote[0].Close[index].Value;
                ulong volume = result.Indicators.Quote[0].Volume[index].Value;
                lastPrice = new HistoricalPrice()
                {
                    Date = dt,
                    Close = price,
                    Volume = volume,
                };
                return true;
            }
            else
            {
                lastPrice = null;
                return false;
            }
        }

        public bool TryGetHistoricalPrices(string ticker, Exchange? exchange, DateTime? from, DateTime? to, PriceInterval priceInterval, out PriceList prices, out string errorMessage)
        {

            double fromValue;
            if(from.HasValue)
            {
                fromValue = (from.Value - DATE_1970).TotalSeconds;
            }
            else
            {
                fromValue = (FIRST_DATE - DATE_1970).TotalSeconds;
            }

            double toValue;
            if (to.HasValue)
            {
                toValue = (to.Value - DATE_1970).TotalSeconds;
            }
            else
            {
                toValue = (DateTime.Now - DATE_1970).TotalSeconds;
            }


            string content = YahooApiCaller.GetHistoricalPrices(ticker, fromValue, toValue, priceInterval);
            string[] lines = content.Split('\n');
            prices = new PriceList();
            for (int i = 1; i < lines.Length; i++)
            {
                HistoricalPrice p = HistoricalPrice.From(lines[i]);
                prices.Add(p);
            }
            errorMessage = "ok";
            return true;
        }

        

        public bool TryGetStatistics(string ticker, Exchange? exchange, out string message)
        {
            //https://query2.finance.yahoo.com/ws/fundamentals-timeseries/v1/finance/timeseries/AAPL?lang=en-US&region=US&symbol=AAPL&padTimeSeries=true&type=annualMarketCap%2CtrailingMarketCap%2CannualEnterpriseValue%2CtrailingEnterpriseValue%2CannualPeRatio%2CtrailingPeRatio%2CannualForwardPeRatio%2CtrailingForwardPeRatio%2CannualPegRatio%2CtrailingPegRatio%2CannualPsRatio%2CtrailingPsRatio%2CannualPbRatio%2CtrailingPbRatio%2CannualEnterprisesValueRevenueRatio%2CtrailingEnterprisesValueRevenueRatio%2CannualEnterprisesValueEBITDARatio%2CtrailingEnterprisesValueEBITDARatio&merge=false&period1=493590046&period2=1583148534&corsDomain=finance.yahoo.com
            throw new NotImplementedException();
        }

        public bool TryGetIndexData(MarketIndex index, out Dictionary<string, decimal> tickersProportions, out string message)
        {
            throw new NotImplementedException();
        }

        public bool TryGetIndexesData(out string message)
        {
            //https://finance.yahoo.com/_finance_doubledown/api/resource/finance.market-summary;fields=shortName%2CregularMarketPrice%2CregularMarketChange%2CregularMarketChangePercent?bkt=%5B%22fd-exp-wilson%22%2C%22fd-jarvis-strm%22%2C%22fdw-ssvrm-test1%22%5D&device=desktop&feature=adsMigration%2CcanvassOffnet%2CccOnMute%2Cdebouncesearch100%2CdeferDarla%2CecmaModern%2CemptyServiceWorker%2CenableCMP%2CenableConsentData%2CenableTheming%2CenableNavFeatureCue%2CenableFeatureTours%2CenableFreeFinRichSearch%2CenableGuceJs%2CenableGuceJsOverlay%2CenableNewResearchInsights%2CenablePremiumSingleCTA%2CenablePremiumScreeners%2CenablePrivacyUpdate%2CenableVideoURL%2CenableYahooSans%2CnewContentAttribution%2CnewLogo%2CoathPlayer%2CrelatedVideoFeature%2CthreeAmigos%2CwaferHeader%2CvideoNativePlaylist%2CenableCCPAFooter%2Clivecoverage%2CdarlaFirstRenderingVisible%2CenableTradeit%2CenableFeatureBar%2CenableSearchEnhancement%2CenableUserSentiment%2CenableBankrateWidget%2CncpHpStream%2Cload6Items%2CcanvassReplies%2CresearchFilter%2CenableSingleRail%2CenablePremiumFinancials%2CenhanceAddToWL%2CsponsoredAds%2CenableStageAds%2CenableTradeItLinkBrokerSecondaryPromo%2CpremiumPromoHeader%2CenableQspPremiumPromoSmall%2CclientDelayNone%2CthreeAmigosMabEnabled%2CthreeAmigosAdsEnabledAndStreamIndex0%2CstreamBucketIdWilson%2CserverSideVRM&intl=us&lang=en-US&partner=none&prid=70919d5f5pp0i&region=US&site=finance&tz=America%2FLos_Angeles&ver=0.102.3297&returnMeta=true
            throw new NotImplementedException();
        }

        public bool TryGetOptionsChain(string ticker, Exchange? exchange, out OptionsChain optionChain, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool TryGetOptionsChainWithTheoricalValue(string ticker, Exchange? exchange, double lastPrice, out OptionsChain optionsChain, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool TryGetOptionsChainWithTheoricalValue(string ticker, Exchange? exchange, double lastPrice, PriceList historicalPrices, out OptionsChain optionsChain, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool TryGetOptionData(string underlyingTicker, ushort year, ushort month, ushort day, out Option option)
        {
            /*
             * Examples
             * 
             * JPM Jan 2022 95.000 call
             * https://finance.yahoo.com/quote/JPM220121C00095000/
             * 
             * JPM Jan 2022 80.000 call
             * https://finance.yahoo.com/quote/JPM220121C00080000/
             * 
             * AMD Jan 21 2022 90 Call	
             * https://finance.yahoo.com/quote/AMD220121C00090000/
             * https://query1.finance.yahoo.com/v8/finance/chart/AMD220121C00090000?region=US&lang=en-US&includePrePost=false&interval=2m&range=1d&corsDomain=finance.yahoo.com&.tsrc=finance
             * 
             * Obvously, it will work until expiration
             */

            throw new NotImplementedException();
        }
    }
}
