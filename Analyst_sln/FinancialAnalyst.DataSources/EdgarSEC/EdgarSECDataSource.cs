using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.DataSources.Edgar
{
    public class EdgarSECDataSource : IDataSource
    {
        public bool TryGetAssetData(string ticker, Exchange? market, out AssetBase asset, out string errorMessage)
        {
            throw new NotImplementedException();
        }
    }
}
