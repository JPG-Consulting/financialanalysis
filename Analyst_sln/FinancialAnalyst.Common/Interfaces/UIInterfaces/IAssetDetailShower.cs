using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Portfolios;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Interfaces.UIInterfaces
{
    public interface IAssetDetailShower
    {
        void ShowAssetDetail(AssetAllocation asset);
    }
}
