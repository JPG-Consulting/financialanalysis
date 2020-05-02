using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Assets
{
    public class Cash:AssetBase
    {
        public override AssetClass AssetClass
        {
            get { return AssetClass.Cash; }
            protected set { }
        }

        public Cash(string ticker):base(ticker)
        {

        }
    }
}
