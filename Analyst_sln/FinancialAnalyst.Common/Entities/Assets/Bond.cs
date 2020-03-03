using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Assets
{
    [Serializable]
    public class Bond:AssetBase
    {
        public override AssetType AssetType { get { return AssetType.Bond; } }

    }
}
