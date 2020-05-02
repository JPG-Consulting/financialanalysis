using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Assets
{
    [Serializable]
    public class ETF : AssetBase
    {
        public override AssetClass AssetClass 
        { 
            get { return AssetClass.ETF; } 
            protected set { } 
        }

        public ETF(string ticker) : base(ticker)
        {

        }
    }
}
