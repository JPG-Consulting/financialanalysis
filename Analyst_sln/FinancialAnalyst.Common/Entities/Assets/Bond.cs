using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Assets
{
    [Serializable]
    public class Bond:AssetBase
    {
        public override AssetClass AssetClass 
        { 
            get { return AssetClass.Bond; } 
            protected set { } 
        }

        public Bond(string ticker) : base(ticker)
        {

        }
    }
}
