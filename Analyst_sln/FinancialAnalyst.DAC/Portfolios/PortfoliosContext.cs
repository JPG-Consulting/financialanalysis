using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.DataAccess.Portfolios
{
    public interface IPortfoliosContext
    {

    }
    public class PortfoliosContext : DbContext, IPortfoliosContext
    {
    }
}
