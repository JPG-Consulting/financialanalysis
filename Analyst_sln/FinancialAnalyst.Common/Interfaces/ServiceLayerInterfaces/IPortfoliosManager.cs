using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Portfolios;
using FinancialAnalyst.Common.Entities.Users;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces
{
    public interface IPortfoliosManager
    {
        bool GetPortfoliosByUserName(string username, out IEnumerable<Portfolio> portfolios, out string message);
        bool CreatePortfolio(string userName, string portfolioname, Stream fileStream, bool firstRowIsInitalBalance, bool overrideIfExists, out Portfolio portfolio, out string message);
        bool Update(int portfolioId, decimal marketValue);
        bool Update(AssetAllocation allocation, out decimal? marketValue);
    }
}
