using FinancialAnalyst.Common.Entities.Portfolios;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Users
{
    public class User
    {
        
        int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public List<Portfolio> Portfolios { get; set; }

        public List<Transaction> Transactions { get; set; }

    }
}
