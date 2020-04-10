using FinancialAnalyst.Common.Entities.Portfolios;
using FinancialAnalyst.Common.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinancialAnalyst.DataAccess.Portfolios
{
    public interface IPortfoliosContext
    {
        int Add(Portfolio portfolio);
        int Add(Transaction transaction);
        int Add(PortfolioBalance pb);
        User GetUser(string userName);
        IEnumerable<Portfolio> GetPortfoliosByUserName(string username);
    }
    public class PortfoliosContext : DbContext, IPortfoliosContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Portfolio> Portfolios { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<PortfolioBalance> PortfolioBalances { get; set; }

        public PortfoliosContext()
        {

        }


        public PortfoliosContext(DbContextOptions<PortfoliosContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (options.IsConfigured == false)
            {
                string connString = GetConnectionString();
                if (string.IsNullOrEmpty(connString))
                {
                    throw new ArgumentException("It can't load Portfolios database, there is no connection string configured");

                }

                options.UseSqlServer(connString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>();
            modelBuilder.Entity<Portfolio>();
            modelBuilder.Entity<Transaction>();
            modelBuilder.Entity<PortfolioBalance>();
        }

        public static string GetConnectionString()
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            var configuration = configurationBuilder.Build();
            string connString = configuration.GetConnectionString("FA_Portfolios");
            return connString;
        }

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

        public User GetUser(string userName)
        {
            var users = Users.Where(u => u.UserName == userName).ToList();
            if (users.Count == 1)
                return users[0];
            else
                return null;
        }

        public IEnumerable<Portfolio> GetPortfoliosByUserName(string username)
        {
            User user = GetUser(username);
            if (user == null)
                return new List<Portfolio>();
            else
                return Portfolios.Where(p => p.UserId == user.Id).ToList();
        }

    }
}
