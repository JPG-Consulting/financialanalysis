using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Interfaces
{
    public interface IDataSource
    {
        bool TryGetAssetData(string ticker, Exchange? market,out AssetBase asset,out string errorMessage);
    }
}
