using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Assets
{
    [Serializable]
    public class ETF : AssetBase
    {
        public override AssetType AssetType { get { return AssetType.ETF; } }
    }
}
