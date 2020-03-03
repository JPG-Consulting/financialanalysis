using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Assets
{
    [Serializable]
    public class Option : AssetBase
    {
        public override AssetType AssetType { get { return AssetType.Option; } }
        public AssetBase UnderlyingAsset { get; set; }
    }
}
