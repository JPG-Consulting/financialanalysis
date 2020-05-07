using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FinancialAnalyst.DataSources.FinancialDataSources.Yahoo
{
    /// <summary>
    /// Thanks to:
    /// https://quicktype.io/csharp/
    /// https://app.quicktype.io/?l=csharp
    /// </summary>
    public partial class YahooChartResponse
    {
        [JsonProperty("chart")]
        public Chart Chart { get; set; }
    }

    public partial class Chart
    {
        [JsonProperty("result")]
        public ChartResult[] Result { get; set; }

        [JsonProperty("error")]
        public object Error { get; set; }
    }

    public partial class ChartResult
    {
        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("timestamp")]
        public long[] TimeStampsAsLong { get; set; }

        [JsonProperty("indicators")]
        public Indicators Indicators { get; set; }
    }

    public partial class Indicators
    {
        [JsonProperty("quote")]
        public Quote[] Quote { get; set; }
    }

    public partial class Quote
    {
        [JsonProperty("open")]
        public decimal?[] Open { get; set; }

        [JsonProperty("high")]
        public decimal?[] High { get; set; }

        [JsonProperty("low")]
        public decimal?[] Low { get; set; }

        [JsonProperty("volume")]
        public ulong?[] Volume { get; set; }

        [JsonProperty("close")]
        public decimal?[] Close { get; set; }
    }

    public partial class Meta
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("exchangeName")]
        public string ExchangeName { get; set; }

        [JsonProperty("instrumentType")]
        public string InstrumentType { get; set; }

        [JsonProperty("firstTradeDate")]
        public long FirstTradeDate { get; set; }

        [JsonProperty("regularMarketTime")]
        public long RegularMarketTime { get; set; }

        [JsonProperty("gmtoffset")]
        public long Gmtoffset { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("exchangeTimezoneName")]
        public string ExchangeTimezoneName { get; set; }

        [JsonProperty("regularMarketPrice")]
        public double RegularMarketPrice { get; set; }

        [JsonProperty("chartPreviousClose")]
        public double ChartPreviousClose { get; set; }

        [JsonProperty("previousClose")]
        public double PreviousClose { get; set; }

        [JsonProperty("scale")]
        public long Scale { get; set; }

        [JsonProperty("priceHint")]
        public long PriceHint { get; set; }

        [JsonProperty("currentTradingPeriod")]
        public CurrentTradingPeriod CurrentTradingPeriod { get; set; }

        [JsonProperty("tradingPeriods")]
        public Post[][] TradingPeriods { get; set; }

        [JsonProperty("dataGranularity")]
        public string DataGranularity { get; set; }

        [JsonProperty("range")]
        public string Range { get; set; }

        [JsonProperty("validRanges")]
        public string[] ValidRanges { get; set; }
    }

    public partial class CurrentTradingPeriod
    {
        [JsonProperty("pre")]
        public Post Pre { get; set; }

        [JsonProperty("regular")]
        public Post Regular { get; set; }

        [JsonProperty("post")]
        public Post Post { get; set; }
    }

    public partial class Post
    {
        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("start")]
        public long Start { get; set; }

        [JsonProperty("end")]
        public long End { get; set; }

        [JsonProperty("gmtoffset")]
        public long Gmtoffset { get; set; }
    }

    //public partial class YahooChartResponse
    //{
    //    public static YahooChartResponse FromJson(string json) => JsonConvert.DeserializeObject<YahooChartResponse>(json, QuickType.Converter.Settings);
    //}

    //public static class Serialize
    //{
    //    public static string ToJson(this YahooChartResponse self) => JsonConvert.SerializeObject(self, QuickType.Converter.Settings);
    //}

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
