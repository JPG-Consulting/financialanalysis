using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Assets
{
    [Serializable]
    public class Option : AssetBase
    {
        public override AssetClass AssetClass
        {
            get { return AssetClass.Option; }
            protected set { }
        }

        [Required]
        public AssetBase UnderlyingAsset { get; set; }
        public double? Change { get; set; }
        public double? Bid { get; set; }
        public double? Ask { get; set; }
        public int? Volume { get; set; }
        public int? OpenInterest { get; set; }

        [Required]
        public double Strike { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
        public double? TheoricalValue { get; set; }

        [NotMapped]
        public bool IsCall { get { return _optionClass == OptionClass.Call; } }
        [NotMapped]
        public bool IsPut { get { return _optionClass == OptionClass.Put; } }

        public OptionClass OptionClass { get { return _optionClass; } }

        

        private readonly OptionClass _optionClass;

        public Option(OptionClass optionClass, string ticker):base(ticker)
        {
            _optionClass = optionClass;
        }

        public void SetLast(dynamic last)
        {
            string str = last;
            if (decimal.TryParse(str, out decimal result))
                LastPrice = result;
            else
                LastPrice = null;
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
