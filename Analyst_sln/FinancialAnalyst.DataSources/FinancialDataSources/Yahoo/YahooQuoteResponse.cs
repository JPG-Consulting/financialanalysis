using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.DataSources.FinancialDataSources.Yahoo
{
    /// <summary>
    /// Thanks to
    /// http://json2csharp.com/
    /// </summary>
    public class YahooQuoteResponse
    {
        public QuoteResponse quoteResponse { get; set; }
    }


    public class QuoteResponse
    {
        public List<QuoteResult> result { get; set; }
        public object error { get; set; }
    }

    public class QuoteResult
    {
        public string symbol { get; set; }
        public TwoHundredDayAverageChangePercent twoHundredDayAverageChangePercent { get; set; }
        public DividendDate dividendDate { get; set; }
        public FiftyTwoWeekLowChangePercent fiftyTwoWeekLowChangePercent { get; set; }
        public string language { get; set; }
        public RegularMarketDayRange regularMarketDayRange { get; set; }
        public EarningsTimestampEnd earningsTimestampEnd { get; set; }
        public EpsForward epsForward { get; set; }
        public RegularMarketDayHigh regularMarketDayHigh { get; set; }
        public TwoHundredDayAverageChange twoHundredDayAverageChange { get; set; }
        public TwoHundredDayAverage twoHundredDayAverage { get; set; }
        public AskSize askSize { get; set; }
        public BookValue bookValue { get; set; }
        public FiftyTwoWeekHighChange fiftyTwoWeekHighChange { get; set; }
        public MarketCap marketCap { get; set; }
        public FiftyTwoWeekRange fiftyTwoWeekRange { get; set; }
        public FiftyDayAverageChange fiftyDayAverageChange { get; set; }
        public long firstTradeDateMilliseconds { get; set; }
        public AverageDailyVolume3Month averageDailyVolume3Month { get; set; }
        public int exchangeDataDelayedBy { get; set; }
        public TrailingAnnualDividendRate trailingAnnualDividendRate { get; set; }
        public FiftyTwoWeekLow fiftyTwoWeekLow { get; set; }
        public RegularMarketVolume regularMarketVolume { get; set; }
        public string market { get; set; }
        public PostMarketPrice postMarketPrice { get; set; }
        public string messageBoardId { get; set; }
        public int priceHint { get; set; }
        public int sourceInterval { get; set; }
        public string exchange { get; set; }
        public RegularMarketDayLow regularMarketDayLow { get; set; }
        public string shortName { get; set; }
        public string region { get; set; }
        public FiftyDayAverageChangePercent fiftyDayAverageChangePercent { get; set; }
        public string fullExchangeName { get; set; }
        public EarningsTimestampStart earningsTimestampStart { get; set; }
        public string financialCurrency { get; set; }
        public int gmtOffSetMilliseconds { get; set; }
        public RegularMarketOpen regularMarketOpen { get; set; }
        public RegularMarketTime regularMarketTime { get; set; }
        public RegularMarketChangePercent regularMarketChangePercent { get; set; }
        public TrailingAnnualDividendYield trailingAnnualDividendYield { get; set; }
        public string quoteType { get; set; }
        public AverageDailyVolume10Day averageDailyVolume10Day { get; set; }
        public FiftyTwoWeekLowChange fiftyTwoWeekLowChange { get; set; }
        public FiftyTwoWeekHighChangePercent fiftyTwoWeekHighChangePercent { get; set; }
        public bool tradeable { get; set; }
        public PostMarketTime postMarketTime { get; set; }
        public string currency { get; set; }
        public SharesOutstanding sharesOutstanding { get; set; }
        public RegularMarketPreviousClose regularMarketPreviousClose { get; set; }
        public FiftyTwoWeekHigh fiftyTwoWeekHigh { get; set; }
        public string exchangeTimezoneName { get; set; }
        public PostMarketChangePercent postMarketChangePercent { get; set; }
        public BidSize bidSize { get; set; }
        public RegularMarketChange regularMarketChange { get; set; }
        public FiftyDayAverage fiftyDayAverage { get; set; }
        public string exchangeTimezoneShortName { get; set; }
        public string marketState { get; set; }
        public RegularMarketPrice regularMarketPrice { get; set; }
        public PostMarketChange postMarketChange { get; set; }
        public ForwardPE forwardPE { get; set; }
        public EarningsTimestamp earningsTimestamp { get; set; }
        public Ask ask { get; set; }
        public EpsTrailingTwelveMonths epsTrailingTwelveMonths { get; set; }
        public Bid bid { get; set; }
        public bool triggerable { get; set; }
        public PriceToBook priceToBook { get; set; }
        public string longName { get; set; }
    }

    #region All these classes can be the same
    public class TwoHundredDayAverageChangePercent
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class DividendDate
    {
        public int raw { get; set; }
        public string fmt { get; set; }
        public string longFmt { get; set; }
    }

    public class FiftyTwoWeekLowChangePercent
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class RegularMarketDayRange
    {
        public string raw { get; set; }
        public string fmt { get; set; }
    }

    public class EarningsTimestampEnd
    {
        public int raw { get; set; }
        public string fmt { get; set; }
        public string longFmt { get; set; }
    }

    public class EpsForward
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class RegularMarketDayHigh
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class TwoHundredDayAverageChange
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class TwoHundredDayAverage
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class AskSize
    {
        public int raw { get; set; }
        public string fmt { get; set; }
        public string longFmt { get; set; }
    }

    public class BookValue
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class FiftyTwoWeekHighChange
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class MarketCap
    {
        public long raw { get; set; }
        public string fmt { get; set; }
        public string longFmt { get; set; }
    }

    public class FiftyTwoWeekRange
    {
        public string raw { get; set; }
        public string fmt { get; set; }
    }

    public class FiftyDayAverageChange
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class AverageDailyVolume3Month
    {
        public int raw { get; set; }
        public string fmt { get; set; }
        public string longFmt { get; set; }
    }

    public class TrailingAnnualDividendRate
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class FiftyTwoWeekLow
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class RegularMarketVolume
    {
        public int raw { get; set; }
        public string fmt { get; set; }
        public string longFmt { get; set; }
    }

    public class PostMarketPrice
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class RegularMarketDayLow
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class FiftyDayAverageChangePercent
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class EarningsTimestampStart
    {
        public int raw { get; set; }
        public string fmt { get; set; }
        public string longFmt { get; set; }
    }

    public class RegularMarketOpen
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class RegularMarketTime
    {
        public int raw { get; set; }
        public string fmt { get; set; }
    }

    public class RegularMarketChangePercent
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class TrailingAnnualDividendYield
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class AverageDailyVolume10Day
    {
        public int raw { get; set; }
        public string fmt { get; set; }
        public string longFmt { get; set; }
    }

    public class FiftyTwoWeekLowChange
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class FiftyTwoWeekHighChangePercent
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class PostMarketTime
    {
        public int raw { get; set; }
        public string fmt { get; set; }
    }

    public class SharesOutstanding
    {
        public long raw { get; set; }
        public string fmt { get; set; }
        public string longFmt { get; set; }
    }

    public class RegularMarketPreviousClose
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class FiftyTwoWeekHigh
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class PostMarketChangePercent
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class BidSize
    {
        public int raw { get; set; }
        public string fmt { get; set; }
        public string longFmt { get; set; }
    }

    public class RegularMarketChange
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class FiftyDayAverage
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class RegularMarketPrice
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class PostMarketChange
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class ForwardPE
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class EarningsTimestamp
    {
        public int raw { get; set; }
        public string fmt { get; set; }
        public string longFmt { get; set; }
    }

    public class Ask
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class EpsTrailingTwelveMonths
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class Bid
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }

    public class PriceToBook
    {
        public double raw { get; set; }
        public string fmt { get; set; }
    }
    #endregion

}
