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
        Portfolio GetPortfolioById(int portfolioId);
        IEnumerable<Portfolio> GetPortfoliosByUserName(string username);
        Portfolio GetPortfoliosByUserNameAndPortfolioName(string userName, string portfolioname);
        void Update(Portfolio portfolio);
        void Update(AssetAllocation allocation);
        void DeleteAssetAllocations(Portfolio portfolio);
        void DeletePortfolio(Portfolio portfolio);
        
        
    }
    public class PortfoliosContext : DbContext, IPortfoliosContext
    {
        //To add a new table, execute command in Package Manager Console:
        //dotnet ef migrations add <NameOfMigration> --project FinancialAnalyst.DataAccessCore
        //Then, copy migration from DataAccessCore to DataAccess (.Net Standard)

        public DbSet<User> Users { get; set; }

        public DbSet<Portfolio> Portfolios { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<PortfolioBalance> PortfolioBalances { get; set; }

        public DbSet<AssetAllocation> AssetAllocations { get; set; }

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
            modelBuilder.Entity<Portfolio>().Ignore(p => p.CashPercentage);
            modelBuilder.Entity<Transaction>();
            modelBuilder.Entity<PortfolioBalance>();
            modelBuilder.Entity<AssetAllocation>();
        }

        public static string GetConnectionString()
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            var configuration = configurationBuilder.Build();
            string connString = configuration.GetConnectionString("FA_Portfolios");
            return connString;
        }

        #region CRUD

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

        public Portfolio GetPortfolioById(int portfolioId)
        {
            return Portfolios.Where(p => p.Id == portfolioId).SingleOrDefault();
        }

        public IEnumerable<Portfolio> GetPortfoliosByUserName(string username)
        {
            User user = GetUser(username);
            if (user == null)
                return new List<Portfolio>();
            else
                return Portfolios.Include(p => p.Transactions).Include(p => p.AssetAllocations).Include(p => p.Balances).Where(p => p.UserId == user.Id).ToList();
        }

        public Portfolio GetPortfoliosByUserNameAndPortfolioName(string userName, string portfolioname)
        {
            User user = GetUser(userName);
            if (user == null)
                return null;
            else
                return Portfolios.Include(p => p.Transactions).Include(p => p.AssetAllocations).Include(p => p.Balances).Where(p => p.UserId == user.Id && p.Name == portfolioname).SingleOrDefault();
        }

        public void Update(Portfolio portfolio)
        {
            Entry(portfolio).State = EntityState.Modified;
            SaveChanges();
        }

        public void Update(AssetAllocation allocation)
        {
            //TODO: FALTA CAMPO FECHA DE ACTUALIZACION

            AssetAllocations.Attach(allocation);
            Entry(allocation).State = EntityState.Modified;
            SaveChanges();
        }

        public void DeletePortfolio(Portfolio portfolio)
        {
            Portfolios.Remove(portfolio);
            SaveChanges();
        }

        public void DeleteAssetAllocations(Portfolio portfolio)
        {
            List<AssetAllocation> aas = AssetAllocations.Where(aa => aa.PortfolioId == portfolio.Id).ToList();
            AssetAllocations.RemoveRange(aas);
            SaveChanges();
        }
        #endregion
    }
}
