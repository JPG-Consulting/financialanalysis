using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Assets.Options;
using FinancialAnalyst.Common.Entities.Portfolios;
using FinancialAnalyst.Common.Entities.Prices;
using FinancialAnalyst.Common.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinancialAnalyst.DataAccess.Portfolios
{
    public interface IPortfoliosContext
    {
        AssetBase CreateAsset(AssetClass assetClass, string ticker, OptionClass? optionClass = null, string underlyingTicker = null, DateTime? expiration = null, double? strike = null);
        Portfolio CreatePortfolio(User user, string portfolioname, bool isSimulated, Tuple<string, AssetClass>[] tuples);
        Portfolio CreatePortfolio(User user, string portfolioname);
        int AddBalance(PortfolioBalance pb);
        User GetUser(string userName);
        Portfolio GetPortfolioById(int portfolioId);
        IEnumerable<Portfolio> GetPortfoliosByUserName(string username);
        Portfolio GetPortfoliosByUserNameAndPortfolioName(string userName, string portfolioname);
        AssetBase GetAssetyBy(AssetClass assetClass, string symbol);
        AssetAllocation GetAssetAllocationBy(int assetAllocationId);
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

        #region DBSets
        public DbSet<User> Users { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<PortfolioBalance> PortfolioBalances { get; set; }
        public DbSet<AssetAllocation> AssetAllocations { get; set; }
        public DbSet<HistoricalPrice> HistoricalPrices { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<ETF> ETFs { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Bond> Bonds { get; set; }
        public DbSet<Cash> Cash { get; set; }
        public DbSet<AssetBase> Assets { get; set; }
        #endregion

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
            modelBuilder.Entity<PortfolioBalance>();
            modelBuilder.Entity<Transaction>();
            modelBuilder.Entity<AssetAllocation>();
            modelBuilder.Entity<HistoricalPrice>();

            //TPH: table-per-hierarchy
            //https://docs.microsoft.com/en-us/ef/core/modeling/inheritance
            modelBuilder.Entity<Option>(o =>
            {
                o.Property(oo => oo.OptionClass);
                o.HasOne(oo => oo.UnderlyingAsset).WithMany().IsRequired().OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<AssetBase>(a1 =>
            {
                a1.HasKey(a2 => a2.Id);
                a1.HasIndex(a2 => a2.Ticker).IsUnique();
                a1.HasDiscriminator<AssetClass>(a2 => a2.AssetClass)
                    .HasValue<Stock>(AssetClass.Stock)
                    .HasValue<ETF>(AssetClass.ETF)
                    .HasValue<Bond>(AssetClass.Bond)
                    .HasValue<Option>(AssetClass.Option)
                    .HasValue<Cash>(AssetClass.Cash)
                    ;
                
            });

            

            //TPT: Table-Per-Type
            //https://www.thinktecture.com/en/entity-framework-core/table-per-type-inheritance-support-part-1-code-first/
            //TODO: ver si con TPT queda mejor la BD
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

        public AssetBase CreateAsset(AssetClass assetClass, string ticker, OptionClass? optionClass = null, string underlyingTicker = null, DateTime? expiration = null, double? strike = null)
        {
            switch (assetClass)
            {
                case AssetClass.Stock:
                    Stock stock = new Stock(ticker);
                    SaveAsset(stock);
                    return stock;
                case AssetClass.ETF:
                    ETF etf = new ETF(ticker);
                    SaveAsset(etf);
                    return etf;
                case AssetClass.Bond:
                    Bond bond = new Bond(ticker);
                    SaveAsset(bond);
                    return bond;
                case AssetClass.Option:
                    if (optionClass.HasValue)
                    {
                        Option option = new Option(optionClass.Value,ticker);
                        Stock unserlyingAsset = (Stock)GetAssetyBy(AssetClass.Stock, underlyingTicker);
                        if (unserlyingAsset == null)
                        {
                            unserlyingAsset = new Stock(underlyingTicker);
                            SaveAsset(unserlyingAsset);
                        }
                        option.UnderlyingAsset = unserlyingAsset;
                        option.ExpirationDate = expiration.Value;
                        option.Strike = strike.Value;
                        SaveAsset(option);
                        return option;
                    }
                    else
                    {
                        throw new ArgumentException("Must provide option class (call other put), underlying asset, strike and expiration when asset is an option");
                    }
                case AssetClass.Cash:
                    Cash cash = new Cash(ticker);
                    SaveAsset(cash);
                    return cash;
                default:
                    throw new NotImplementedException($"There is no DBSet AssetClass={assetClass.ToString()}");
            }
        }
        

        public Portfolio CreatePortfolio(User user, string portfolioname, bool isSimulated, Tuple<string, AssetClass>[] tuples)
        {
            Portfolio portfolio = new Portfolio()
            {
                Name = portfolioname,
                AssetAllocations = new List<AssetAllocation>(),
                UserId = user.Id,
            };
            Portfolios.Add(portfolio);
            SaveChanges();

            foreach (var tuple in tuples)
            {
                AssetBase assetBase = GetAssetyBy(tuple.Item2, tuple.Item1);
                if (assetBase == null)
                    assetBase = CreateAsset(tuple.Item2, tuple.Item1);
                AssetAllocation assetAllocation = new AssetAllocation()
                {
                    Asset = assetBase,
                    PortfolioId = portfolio.Id,
                };
                AssetAllocations.Add(assetAllocation);
                portfolio.AssetAllocations.Add(assetAllocation);

            }
            portfolio.UserId = user.Id;
            portfolio.IsSimulated = true;
            Update(portfolio);
            return portfolio;
        }

        public Portfolio CreatePortfolio(User user, string portfolioname)
        {
            Portfolio portfolio = new Portfolio()
            {
                UserId = user.Id,
                Name = portfolioname,
            };
            Portfolios.Add(portfolio);

            //https://docs.microsoft.com/en-us/ef/core/saving/related-data
            //TODO: falta resolver el cascade insert, es decir: si existe obtnerlo y si no crearlo

            SaveChanges();
            return portfolio;
        }
        
        public int AddBalance(PortfolioBalance balance)
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
            {
                var portfolios = Portfolios
                    .Include(p => p.AssetAllocations)
                    .ThenInclude(a => a.Asset)
                    .Include(p => p.Balances)
                    .Include(p => p.Transactions)
                    .ThenInclude(t => t.Asset)
                    .Where(p => p.UserId == user.Id).ToList();

                return portfolios;
            }
        }

        public Portfolio GetPortfoliosByUserNameAndPortfolioName(string userName, string portfolioname)
        {
            User user = GetUser(userName);
            if (user == null)
                return null;
            else
                return Portfolios.Include(p => p.Transactions).Include(p => p.AssetAllocations).Include(p => p.Balances).Where(p => p.UserId == user.Id && p.Name == portfolioname).SingleOrDefault();
        }

        public AssetBase GetAssetyBy(AssetClass assetClass, string symbol)
        {
            IEnumerable<AssetBase> dbset = GetDBSetFor(assetClass);
            return dbset.Where(a => a.Ticker.ToUpper() == symbol.ToUpper()).SingleOrDefault();
        }
        
        public AssetAllocation GetAssetAllocationBy(int assetAllocationId)
        {
            return AssetAllocations
                .Include(aa=> aa.Asset)
                .ThenInclude( asset => ((Option)asset).UnderlyingAsset)
                .Where(aa => aa.Id == assetAllocationId)
                .SingleOrDefault();
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



        private IEnumerable<AssetBase> GetDBSetFor(AssetClass assetClass)
        {
            switch (assetClass)
            {
                case AssetClass.Stock:
                    return Stocks;
                case AssetClass.ETF:
                    return ETFs;
                case AssetClass.Bond:
                    return Bonds;
                case AssetClass.Option:
                    return Options;
                case AssetClass.Cash:
                    return Cash;
                default:
                    throw new NotImplementedException($"There is no DBSet AssetClass={assetClass.ToString()}");
            }
        }

        private void SaveAsset(Stock asset)
        {
            Stocks.Add(asset);
            SaveChanges();
        }

        private void SaveAsset(ETF etf)
        {
            ETFs.Add(etf);
            SaveChanges();
        }

        private void SaveAsset(Bond bond)
        {
            Bonds.Add(bond);
            SaveChanges();
        }

        private void SaveAsset(Option option)
        {
            Options.Add(option);
            SaveChanges();
        }

        private void SaveAsset(Cash cash)
        {
            Cash.Add(cash);
            SaveChanges();
        }
    }
}
