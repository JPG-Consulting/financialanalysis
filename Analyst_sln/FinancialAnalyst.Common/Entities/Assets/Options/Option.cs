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

        [NotMapped]
        public bool IsExotic { get { return _optionClass == OptionClass.Exotic; } }

        public OptionClass OptionClass { get { return _optionClass; } }

        

        private readonly OptionClass _optionClass;

        public Option(OptionClass optionClass, string ticker):base(ticker)
        {
            _optionClass = optionClass;
        }

    }
}
