using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace FinancialAnalyst.DataSources.Yahoo
{
    /// <summary>
    /// Thanks to:
    /// http://www.jarloo.com/get-yahoo-finance-api-data-via-yql/
    /// </summary>
    public class YahooDataSource : IDataSource
    {
        public bool TryGetAssetData(string ticker, Market market, out AssetBase asset, out string errorMessage)
        {
            throw new NotImplementedException();
        }
    }
}
