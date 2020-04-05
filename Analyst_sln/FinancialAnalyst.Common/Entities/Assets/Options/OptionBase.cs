using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Assets
{
    [Serializable]
    public class OptionBase : AssetBase
    {
        /*
        {
          "call": {
            "symbol": "@AXP   200320C00070000",
            "last": "6.00",
            "change": "-10.40",
            "bid": "4.05",
            "ask": "7.95",
            "volume": 5,
            "openinterest": 10,
            "strike": 70.0,
            "expiryDate": "03/20/2020",
            "colour": true
          },
          "put": {
            "symbol": "@AXP   200320P00070000",
            "last": "2.46",
            "change": "1.96",
            "bid": "0.50",
            "ask": "3.30",
            "volume": 80,
            "openinterest": 77,
            "strike": 70.0,
            "expiryDate": "03/20/2020",
            "colour": false
          }
        },
        */

        [JsonIgnore]
        public override AssetType AssetType { get { return AssetType.Option; } }

        public AssetBase UnderlyingAsset { get; set; }
        public string Symbol { get; set; }
        public double? Last { get; set; }
        public double? Change { get; set; }
        public double? Bid { get; set; }
        public double? Ask { get; set; }
        public int? Volume { get; set; }
        public int? OpenInterest { get; set; }
        public double Strike { get; set; }
        public DateTime ExpirationDate { get; set; }
        public double TheoricalValue { get; set; }

        public void SetLast(dynamic last)
        {
            string str = last;
            if (double.TryParse(str, out double result))
                Last = result;
            else
                Last = null;
        }

        public void SetChange(dynamic value)
        {
            string str = value;
            if (double.TryParse(str, out double result))
                Change = result;
            else
                Change = null;
        }

        public void SetBid(dynamic value)
        {
            string str = value;
            if (double.TryParse(str, out double result))
                Bid = result;
            else
                Bid = null;
        }

        public void SetAsk(dynamic value)
        {
            string str = value;
            if (double.TryParse(str, out double result))
                Ask = result;
            else
                Ask = null;
        }

        public void SetVolume(dynamic value)
        {
            string str = value;
            if (int.TryParse(str, out int result))
                Volume = result;
            else
                Volume = null;
        }

        public void SetOpenInterest(dynamic value)
        {
            string str = value;
            if (int.TryParse(str, out int result))
                OpenInterest = result;
            else
                OpenInterest = null;
        }
    }
}
