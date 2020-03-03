using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Assets
{
    [Serializable]
    public abstract class AssetBase
    {
        public abstract AssetType AssetType { get; }
        public string RawData { get; set; }
    }
}
