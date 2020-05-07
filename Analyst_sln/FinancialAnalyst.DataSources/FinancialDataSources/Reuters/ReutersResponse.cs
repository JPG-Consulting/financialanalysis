using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FinancialAnalyst.DataSources.FinancialDataSources.Reuters
{


    public partial class ReutersResponse
    {
        [JsonProperty("retry_in")]
        public long RetryIn { get; set; }

        [JsonProperty("ts")]
        public long Ts { get; set; }

        [JsonProperty("market_data")]
        public MarketData MarketData { get; set; }

        [JsonProperty("ric")]
        public string Ric { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }
    }

    public partial class MarketData
    {
        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        [JsonProperty("fundamental_exchange_name")]
        public string FundamentalExchangeName { get; set; }

        [JsonProperty("ric")]
        public string Ric { get; set; }

        [JsonProperty("exchange_name")]
        public object ExchangeName { get; set; }

        [JsonProperty("last")]
        public string Last { get; set; }

        [JsonProperty("last_time")]
        //public DateTimeOffset LastTime { get; set; }
        public string LastTime { get; set; }

        [JsonProperty("net_change")]
        public string NetChange { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("percent_change")]
        public string PercentChange { get; set; }

        [JsonProperty("modified")]
        public DateTimeOffset Modified { get; set; }

        [JsonProperty("volume")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Volume { get; set; }

        [JsonProperty("day_high")]
        public string DayHigh { get; set; }

        [JsonProperty("day_low")]
        public string DayLow { get; set; }

        [JsonProperty("fiftytwo_wk_high")]
        public string FiftytwoWkHigh { get; set; }

        [JsonProperty("fiftytwo_wk_low")]
        public string FiftytwoWkLow { get; set; }

        [JsonProperty("prev_day_close")]
        public string PrevDayClose { get; set; }

        [JsonProperty("open")]
        public string Open { get; set; }

        [JsonProperty("local_name")]
        public object LocalName { get; set; }

        [JsonProperty("market_cap")]
        public string MarketCap { get; set; }

        [JsonProperty("share_volume_3m")]
        public string ShareVolume3M { get; set; }

        [JsonProperty("beta")]
        public string Beta { get; set; }

        [JsonProperty("eps_excl_extra_ttm")]
        public string EpsExclExtraTtm { get; set; }

        [JsonProperty("pe_excl_extra_ttm")]
        public string PeExclExtraTtm { get; set; }

        [JsonProperty("ps_annual")]
        public string PsAnnual { get; set; }

        [JsonProperty("ps_ttm")]
        public string PsTtm { get; set; }

        [JsonProperty("pcf_share_ttm")]
        public string PcfShareTtm { get; set; }

        [JsonProperty("pb_annual")]
        public string PbAnnual { get; set; }

        [JsonProperty("pb_quarterly")]
        public string PbQuarterly { get; set; }

        [JsonProperty("dividend_yield_indicated_annual")]
        public string DividendYieldIndicatedAnnual { get; set; }

        [JsonProperty("lt_debt_equity_annual")]
        public string LtDebtEquityAnnual { get; set; }

        [JsonProperty("total_debt_equity_annual")]
        public string TotalDebtEquityAnnual { get; set; }

        [JsonProperty("lt_debt_equity_quarterly")]
        public string LtDebtEquityQuarterly { get; set; }

        [JsonProperty("total_debt_equity_quarterly")]
        public string TotalDebtEquityQuarterly { get; set; }

        [JsonProperty("shares_out")]
        public string SharesOut { get; set; }

        [JsonProperty("roe_ttm")]
        public string RoeTtm { get; set; }

        [JsonProperty("roi_ttm")]
        public string RoiTtm { get; set; }

        [JsonProperty("sig_devs")]
        public SigDev[] SigDevs { get; set; }

        [JsonProperty("about")]
        public string About { get; set; }

        [JsonProperty("about_jp")]
        public string AboutJp { get; set; }

        [JsonProperty("website")]
        public object Website { get; set; }

        [JsonProperty("street_address")]
        public string[] StreetAddress { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("phone")]
        public Phone Phone { get; set; }

        [JsonProperty("sector")]
        public string Sector { get; set; }

        [JsonProperty("industry")]
        public string Industry { get; set; }

        [JsonProperty("forward_PE")]
        public string ForwardPe { get; set; }

        [JsonProperty("officers")]
        public ReutersResponse_Officer[] Officers { get; set; }

        [JsonProperty("recommendation")]
        public Recommendation Recommendation { get; set; }

        [JsonProperty("eps_per_year")]
        public PerYear EpsPerYear { get; set; }

        [JsonProperty("revenue_per_year")]
        public PerYear RevenuePerYear { get; set; }

        [JsonProperty("next_event")]
        public NextEvent NextEvent { get; set; }
    }

    public partial class PerYear
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("data")]
        public Datum[] Data { get; set; }
    }

    public partial class Datum
    {
        [JsonProperty("fiscal_year")]
        public long FiscalYear { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }

        [JsonProperty("estimate")]
        public bool Estimate { get; set; }
    }

    public partial class NextEvent
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("time")]
        public DateTimeOffset Time { get; set; }
    }

    public partial class ReutersResponse_Officer
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("rank")]
        public int Rank { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("age")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Age { get; set; }

        [JsonProperty("since")]
        public string Since { get; set; }
    }

    public partial class Phone
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("country_phone_code")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long CountryPhoneCode { get; set; }

        [JsonProperty("city_area_code")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long CityAreaCode { get; set; }

        [JsonProperty("number")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Number { get; set; }
    }

    public partial class Recommendation
    {
        [JsonProperty("unverified_mean")]
        public double UnverifiedMean { get; set; }

        [JsonProperty("preliminary_mean")]
        public double PreliminaryMean { get; set; }

        [JsonProperty("mean")]
        public double Mean { get; set; }

        [JsonProperty("high")]
        public long High { get; set; }

        [JsonProperty("low")]
        public long Low { get; set; }

        [JsonProperty("number_of_recommendations")]
        public long NumberOfRecommendations { get; set; }
    }

    public partial class SigDev
    {
        [JsonProperty("development_id")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long DevelopmentId { get; set; }

        [JsonProperty("last_update")]
        //public DateTimeOffset LastUpdate { get; set; }
        public string LastUpdate { get; set; }

        [JsonProperty("headline")]
        public string Headline { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public partial class Status
    {
        [JsonProperty("code")]
        public long Code { get; set; }
    }

    //public partial class ReutersResponse
    //{
    //    public static ReutersResponse FromJson(string json) => JsonConvert.DeserializeObject<ReutersResponse>(json, QuickType.Converter.Settings);
    //}

    //public static class Serialize
    //{
    //    public static string ToJson(this ReutersResponse self) => JsonConvert.SerializeObject(self, QuickType.Converter.Settings);
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

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }

}
