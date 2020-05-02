using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Assets
{
    public class Future : AssetBase
    {
        public override AssetClass AssetClass
        {
            get { return AssetClass.Futures; }
            protected set { }
        }

        public Future(string ticker) : base(ticker)
        {

        }
    }
}
