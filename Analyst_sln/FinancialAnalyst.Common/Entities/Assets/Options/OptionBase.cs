using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Assets
{
    [Serializable]
    public class OptionBase : AssetBase
    {
        public override AssetType AssetType { get { return AssetType.Option; } }
        public AssetBase UnderlyingAsset { get; set; }

        public string Symbol { get; set; }
        public double Last { get; set; }
        public double Strike { get; set; }
        public DateTime ExpirationDate { get; set; }
        public double TheoricalValue { get; internal set; }
    }
}
