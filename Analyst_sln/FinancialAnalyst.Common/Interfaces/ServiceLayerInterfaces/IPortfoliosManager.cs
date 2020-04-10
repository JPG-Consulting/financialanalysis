using FinancialAnalyst.Common.Entities.Portfolios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces
{
    public interface IPortfoliosManager
    {
        bool Create(string userName, string portfolioname, Stream fileStream, bool firstRowIsInitalBalance, out Portfolio portfolio, out string message);
    }
}
