using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces
{
    public interface IAssetDataDataSource
    {
        bool TryGetAssetData(string ticker, Exchange? exchange, out AssetBase asset, out string errorMessage);
    }
}
