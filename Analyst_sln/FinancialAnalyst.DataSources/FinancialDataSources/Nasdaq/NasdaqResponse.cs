using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.DataSources.FinancialDataSources.Nasdaq
{
    [JsonObject]
    [Serializable]
    internal class NasdaqResponse
    {
        /*
        {
          "data": ...,
          "message": null,
          "status": {
            "rCode": 400,
            "bCodeMessage": [
              {
                "code": 1001,
                "errorMessage": "Symbol not exists."
              }
            ],
            "developerMessage": null
          }
        }
       */

        [JsonProperty("data")]
        public NasdaqResponse_Data Data { get; set; }

        [JsonProperty("message")]
        public object Message { get; set; }

        [JsonProperty("status")]
        public NasdaqResponse_Status Status { get; set; }
    }

    [JsonObject]
    internal class NasdaqResponse_Data
    {
        [JsonProperty("symbol")]
        public string Symbol {get;set;}

        [JsonProperty("isNasdaq100")]
        public bool IsNasdaq100 { get; set; }


        #region Data for Summary
        [JsonProperty("companyName")]
        public string CompanyName {get;set;}

        [JsonProperty("stockType")]
        public string StockType {get;set;}

        [JsonProperty("exchange")]
        public string Exchange {get;set;}

        [JsonProperty("isNasdaqListed")]
        public bool IsNasdaqListed {get;set;}

        [JsonProperty("isHeld")]
        public bool IsHeld {get;set;}

        [JsonProperty("primaryData")]
        public NasdaqResponse_PrimaryData PrimaryData {get;set;}

        [JsonProperty("secondaryData")]
        public object SecondaryData {get;set;}

        [JsonProperty("keyStats")]
        public NasdaqResponse_KeyStats KeyStats { get; set; }

        [JsonProperty("marketStatus")]
        public string MarketStatus { get; set; }

        [JsonProperty("assetClass")]
        public string AssetClass { get; set; }
        #endregion


        #region Data for prices
        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("timeAsOf")]
        public string TimeAsOf { get; set; }

        [JsonProperty("lastSalePrice")]
        public string lastSalePrice { get; set; }

        [JsonProperty("netChange")]
        public string netChange { get; set; }

        [JsonProperty("percentageChange")]
        public string percentageChange { get; set; }

        [JsonProperty("deltaIndicator")]
        public string deltaIndicator { get; set; }

        [JsonProperty("previousClose")]
        public string previousClose { get; set; }

        [JsonProperty("chart")]
        public List<NasdaqResponse_Price> Prices { get; set; }

        #endregion
    }

    [JsonObject]
    internal class NasdaqResponse_PrimaryData
    {
        [JsonProperty("lastSalePrice")]
        public string LastSalePriceAsString { get; set; }

        [JsonProperty("netChange")]
        public string NetChange { get; set; }

        [JsonProperty("percentageChange")]
        public string PercentageChange { get; set; }

        [JsonProperty("deltaIndicator")]
        public string DeltaIndicator { get; set; }

        [JsonProperty("lastTradeTimestamp")]
        public string LastTradeTimestampAsString { get; set; }

        [JsonProperty("isRealTime")]
        public bool IsRealTime { get; set; }
    }

    [JsonObject]
    internal class NasdaqResponse_KeyStats
    {
        [JsonProperty("Volume")]
        public NasdaqResponse_KeyStat Volume { get; set; }

        [JsonProperty("PreviousClose")]
        public NasdaqResponse_KeyStat PreviousClose { get; set; }

        [JsonProperty("OpenPrice")]
        public NasdaqResponse_KeyStat OpenPrice { get; set; }

        [JsonProperty("MarketCap")]
        public NasdaqResponse_KeyStat MarketCap { get; set; }

    }

    [JsonObject]
    internal class NasdaqResponse_KeyStat
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    [JsonObject]
    internal class NasdaqResponse_Price
    {
        [JsonProperty("z")]
        public object DayData { get; set; }

        [JsonProperty("x")]
        public long Day { get; set; }

        [JsonProperty("y")]
        public decimal Price { get; set; }
    }

    [JsonObject]
    internal class NasdaqResponse_Status
    {
        [JsonProperty("rCode")]
        public int Code { get; set; }

        [JsonProperty("bCodeMessage")]
        public List<NasdaqResponse_CodeMessage> CodeMessage { get; set; }

        [JsonProperty("developerMessage")]
        public object DeveloperMessage { get; set; }
    }

    [JsonObject]
    internal class NasdaqResponse_CodeMessage
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }
    }
}
