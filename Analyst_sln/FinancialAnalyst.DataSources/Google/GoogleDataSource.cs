using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.DataSources.Google
{
    /// <summary>
    /// https://developers.google.com/sites/faq_gdata
    /// https://stackoverflow.com/questions/46070126/google-finance-json-stock-quote-stopped-working
    /// </summary>
    public class GoogleDataSource : IDataSource
    {
        public bool TryGetAssetData(string ticker, Exchange? market, out AssetBase asset, out string errorMessage)
        {
            throw new NotImplementedException();
        }
    }
}
