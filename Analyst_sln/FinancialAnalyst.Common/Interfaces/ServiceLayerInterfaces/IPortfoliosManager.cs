using FinancialAnalyst.Common.Entities.Portfolios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces
{
    public interface IPortfoliosManager
    {
        Portfolio Create(string userid, string portfolioname, Stream fileStream, bool firstRowIsInitalBalance);
    }
}
