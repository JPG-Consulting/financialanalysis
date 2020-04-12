using FinancialAnalyst.Common.Entities.Portfolios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces
{
    public interface IPortfoliosManager
    {
        bool GetPortfoliosByUserName(string username, out IEnumerable<Portfolio> portfolios, out string message);
        bool Create(string userName, string portfolioname, Stream fileStream, bool firstRowIsInitalBalance, bool overrideIfExists, out Portfolio portfolio, out string message);
    }
}
