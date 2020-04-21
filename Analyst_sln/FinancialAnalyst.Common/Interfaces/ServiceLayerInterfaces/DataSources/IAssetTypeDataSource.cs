using FinancialAnalyst.Common.Entities.Assets;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.DataSources
{
    public interface IAssetTypeDataSource
    {
        bool TryGetAssetType(string symbol, out AssetType assetType);
    }
}
