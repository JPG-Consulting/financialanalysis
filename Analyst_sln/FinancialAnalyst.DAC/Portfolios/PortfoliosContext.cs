using FinancialAnalyst.Common.Entities.Portfolios;
using FinancialAnalyst.Common.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.DataAccess.Portfolios
{
    public interface IPortfoliosContext
    {
        int Add(Portfolio portfolio);
        int Add(Transaction transaction);
        int Add(PortfolioBalance pb);
        int GetUser(string userName);
    }
    public class PortfoliosContext : DbContext, IPortfoliosContext
    {
        /*
        public DbSet<User> Users { get; set; }

        public DbSet<Portfolio> Portfolios { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<PortfolioBalance> PortfolioBalances { get; set; }
        */

        public List<User> Users = new List<User>();

        public List<Portfolio> Portfolios = new List<Portfolio>();

        public List<Transaction> Transactions = new List<Transaction>();

        public List<PortfolioBalance> PortfolioBalances = new List<PortfolioBalance>();

        public int Add(Portfolio portfolio)
        {
            Portfolios.Add(portfolio);
            SaveChanges();
            return portfolio.Id;
        }
        
        public int Add(Transaction transaction)
        {
            Transactions.Add(transaction);
            SaveChanges();
            return transaction.Id;
        }

        public int Add(PortfolioBalance balance)
        {
            PortfolioBalances.Add(balance);
            SaveChanges();
            return balance.Id;
        }

        public int GetUser(string userName)
        {
            return 1;
        }


        public override int SaveChanges()
        {
            return 1;
        }

    }
}
