using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Portfolios;
using FinancialAnalyst.Common.Entities.Prices;
using FinancialAnalyst.Common.Entities.Users;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.DataSources;
using FinancialAnalyst.DataAccess.Portfolios;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace FinancialAnalyst.Portfolios
{
    public class PortfoliosManager : IPortfoliosManager
    {
        private static readonly IFormatProvider FORMAT_PROVIDER = CultureInfo.GetCultureInfo("en-us");

        private readonly IPortfoliosContext portfoliosContext;
        private readonly IDataSource dataSource;
        public PortfoliosManager(IPortfoliosContext portfoliosContext, IDataSource dataSource)
        {
            this.portfoliosContext = portfoliosContext;
            this.dataSource = dataSource;
        }

        public bool GetPortfoliosByUserName(string username, out IEnumerable<Portfolio> portfolios, out string message)
        {
            portfolios = portfoliosContext.GetPortfoliosByUserName(username);
            foreach(Portfolio portfolio in portfolios)
            {
                if(portfolio.Transactions.Count > 0)
                {
                    //recalculate asset allocations
                    portfoliosContext.DeleteAssetAllocations(portfolio);
                    CalculateAssetAllocations(portfolio);
                    portfoliosContext.Update(portfolio);
                }
            }
            message = "";
            return true;
        }

        /// <summary>
        /// It creates a portfolio from the transactions.
        /// First line of fileStream has to be the initial balance.
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="portfolioname"></param>
        /// <param name="fileStream">It expects a CSV file. First line has to be the initial balance</param>
        /// <returns></returns>
        public bool CreatePortfolio(string userName, string portfolioname, Stream fileStream, bool firstRowIsInitalBalance, bool overrideIfExists, out Portfolio portfolio, out string message)
        {
            List<string[]> transactionsList = new List<string[]>();

            User user = portfoliosContext.GetUser(userName);
            if(user == null)
            {
                portfolio = null;
                message = $"User '{userName}' doesn't exist.";
                return false;
            }

            portfolio = portfoliosContext.GetPortfoliosByUserNameAndPortfolioName(userName, portfolioname);
            int portfolioId;
            if (overrideIfExists && portfolio != null)
            {
                portfoliosContext.DeletePortfolio(portfolio);
                portfolio = null;
            }
            
            if (portfolio == null)
            {
                portfolio = portfoliosContext.CreatePortfolio(user, portfolioname);
                portfolioId = portfolio.Id;
            }
            else
            {
                portfolioId = portfolio.Id;
            }

            fileStream.Position = 0;
            using (StreamReader reader = new StreamReader(fileStream, System.Text.Encoding.UTF8, true))
            {
                //First row is Header
                if (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                }

                if (firstRowIsInitalBalance)
                {
                    if (reader.Peek() >= 0)
                    {
                        string line = reader.ReadLine();
                        string[] fields = line.Split(',');
                        PortfolioBalance pb = PortfolioBalance.From(fields);
                        if (portfolio.Balances.Where(b => b.TransactionCode == pb.TransactionCode).Any() == false)
                        {
                            pb.PortfolioId = portfolioId;
                            portfoliosContext.AddBalance(pb);//And it also adds the balance to the balances collection of the portfolio
                            portfolio.InitialBalance = pb.NetCashBalance;
                        }
                    }
                }
                else
                {
                    //TODO: Get InitialBalance from DataBase   
                }

                //Second to end row are the transactions
                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                    string[] fields = line.Split(',');
                    transactionsList.Add(fields);
                    Transaction newTransaction = Transaction.From(fields);
                    if (newTransaction != null && portfolio.Transactions.Where(t => t.TransactionCode == newTransaction.TransactionCode).Any() == false)
                    {
                        newTransaction.UserId = user.Id;
                        newTransaction.PortfolioId = portfolioId;
                        if (TryGetAsset(newTransaction.Symbol, out AssetBase asset))
                        {
                            if (string.IsNullOrEmpty(newTransaction.Symbol))
                                newTransaction.Symbol = asset.Ticker;
                            newTransaction.Asset = asset;
                        }
                        else
                            newTransaction.Asset = null;
                        portfolio.Transactions.Add(newTransaction);
                        //portfoliosContext.AddTransaction(newTransaction);//And it also adds the transaction to the transactions collection of the portfolio
                    }
                    else
                    {
                        //TODO: Inform to user that transaction wasn't saved
                    }
                }
            }

            CalculateAssetAllocations(portfolio);
            portfoliosContext.Update(portfolio);

            message = $"Portfilio '{portfolioname}' created successfuly.";
            return true;
        }

        public bool Update(int portfolioId, decimal marketValue)
        {
            
            Portfolio portfolio = portfoliosContext.GetPortfolioById(portfolioId);
            portfolio.MarketValue = marketValue;
            portfoliosContext.Update(portfolio);
            return true;
        }

        public bool Update(AssetAllocation assetAllocation, out decimal? marketValue)
        {
            if (assetAllocation.Asset.AssetClass == AssetClass.Stock || assetAllocation.Asset.AssetClass == AssetClass.ETF)
            {
                if (dataSource.TryGetLastPrice(assetAllocation.Asset.Ticker, assetAllocation.Asset.Exchange, assetAllocation.Asset.AssetClass, out HistoricalPrice price, out string message))
                {
                    if (price != null)
                    {
                        marketValue = assetAllocation.UpdateMarketValue(price.Close, price.Date);
                        if (marketValue.HasValue == false)
                            marketValue = assetAllocation.Costs.Value;
                    }
                    else
                    {
                        if (assetAllocation.Costs.HasValue)
                            marketValue = assetAllocation.Costs.Value;
                        else
                            marketValue = null;
                    }
                    portfoliosContext.Update(assetAllocation);
                    return true;
                }
                else
                {
                    marketValue = null;
                    return false;
                }
            }
            else if(assetAllocation.Asset.AssetClass == AssetClass.Option)
            {
                bool ok = dataSource.TryGetCompleteAssetData(assetAllocation.Asset.Ticker, assetAllocation.Asset.Exchange, assetAllocation.Asset.AssetClass, true, false, out AssetBase asset, out string errorMessage);
                if(ok)
                {
                    //OptionsChain chain = stock.OptionsChain.Where(kv => kv.Key == assetAllocation.Asset.As<Option>().MaturityDate)
                    throw new NotImplementedException();
                }
                else
                {
                    throw new NotImplementedException();
                }

            }
            else
            {
                throw new NotImplementedException($"It can't get last price for ticker '{assetAllocation.Asset.Ticker}' (Type='{assetAllocation.Asset.AssetClass}')");
            }
            
        }

        private void CalculateAssetAllocations(Portfolio portfolio)
        {
            decimal initialAmount;
            if (portfolio.InitialBalance.HasValue)
                initialAmount = portfolio.InitialBalance.Value;
            else
            {
                initialAmount = portfolio.Transactions.Sum(t => t.Amount);
                portfolio.InitialBalance = initialAmount;
            }

            decimal cash = initialAmount;
            decimal totalCosts = 0;
            Dictionary<string, AssetAllocation> groups = new Dictionary<string, AssetAllocation>();
            foreach (Transaction t in portfolio.Transactions)
            {
                if(t.CashflowType == CashflowTypes.Bought || t.CashflowType == CashflowTypes.Sell)
                {
                    AssetAllocation assetAllocation;
                    if (groups.ContainsKey(t.Symbol) == false)
                    {
                        assetAllocation = new AssetAllocation() 
                        { 
                            Asset = t.Asset,
                            Costs = 0,
                            Quantity = 0,
                            PortfolioId = portfolio.Id,
                        };
                        groups.Add(t.Symbol, assetAllocation);
                    }
                    else
                        assetAllocation = groups[t.Symbol];


                    /*
                     * //TODO: <<<<<<<<<<<<<<<<<<<<corregir: cantidad, costo y precio.>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                     * Aca estoy calculando el costo
                     * Falta agregar cantidad, costo y el campo amount deberia ser para el precio
                     */
                    if (t.CashflowType == CashflowTypes.Bought)
                    {
                        assetAllocation.Costs += Math.Abs(t.Amount);
                        assetAllocation.Quantity += t.Quantity;
                        totalCosts += Math.Abs(t.Amount);
                    }
                    else
                    {
                        assetAllocation.Costs -= Math.Abs(t.Amount);
                        assetAllocation.Quantity -= t.Quantity;
                        totalCosts -= Math.Abs(t.Amount);
                    }

                    cash += t.Amount;//Bougths are negative and sells are positive, there is no need to use abs()
                    //cash -= t.Commission;//Amount already contains comission.
                    //cash -= t.RegFee;//Amount already contains reg fee.
                }
                else
                {
                    cash += t.Amount;
                }
            }

            if(initialAmount + portfolio.Transactions.Sum(t => t.Amount) == cash)
            {
                //ok
            }
            else
            {
                //error
            }

            foreach(var assetAllocation in groups)
            {
                assetAllocation.Value.Percentage = assetAllocation.Value.Costs / initialAmount * 100;
                portfolio.AssetAllocations.Add(assetAllocation.Value);
            }

            /*
            AssetAllocation cashAllocation = new AssetAllocation();
            cashAllocation.PortfolioId = portfolio.Id;
            cashAllocation.Ticker = "Cash";
            cashAllocation.Costs = cash;
            cashAllocation.Percentage = cash / initialAmount;
            portfolio.AssetAllocations.Add(cashAllocation);
            */

            portfolio.Cash = cash;
            portfolio.TotalCosts = totalCosts;
            portfolio.MarketValue = totalCosts;//TODO: Pending to calculate
            portfolio.IsSimulated = false;
            
        }

        private bool TryGetAsset(string symbol, out AssetBase asset)
        {
            if (string.IsNullOrEmpty(symbol))
            {
                asset = portfoliosContext.GetAssetyBy(AssetClass.Cash, "USD");
                if (asset == null)
                {
                    asset = portfoliosContext.CreateAsset(AssetClass.Cash, "USD");
                }
                return true;
            }

            bool ok = TryGetAssetType(symbol, out AssetClass assetClass, out OptionClass? optionClass, out string underlyingTicker, out DateTime? expiration, out double? strike);
            if (ok)
            {
                asset = portfoliosContext.GetAssetyBy(assetClass, symbol);
                if (asset == null)
                {
                    asset = portfoliosContext.CreateAsset(assetClass, symbol, optionClass, underlyingTicker, expiration, strike);
                }
                return true;
            }
            else
            {
                throw new Exception($"It failed to get asset class from symbol '{symbol}'");
            }
        }

        private bool TryGetAssetType(string symbol, out AssetClass assetClass, out OptionClass? optionClass, out string underlyingTicker, out DateTime? expiration, out double? strike)
        {
            optionClass = null;
            underlyingTicker = null;
            expiration = null;
            strike = null;



            string[] parts = symbol.ToLower().Split(' ');
            if (parts.Length > 1)
            {
                if (parts.Contains("call"))
                {
                    assetClass = AssetClass.Option;
                    optionClass = OptionClass.Call;
                    underlyingTicker = parts[0];
                    expiration = DateTime.ParseExact($"{parts[1]} {parts[2]} {parts[3]}", "MMM dd yyyy", FORMAT_PROVIDER);
                    strike = double.Parse(parts[4], FORMAT_PROVIDER);
                    return true;
                }

                if (parts.Contains("put"))
                {
                    assetClass = AssetClass.Option;
                    optionClass = OptionClass.Put;
                    underlyingTicker = parts[0];
                    expiration = DateTime.ParseExact($"{parts[1]} {parts[2]} {parts[3]}", "MMM dd yyyy", FORMAT_PROVIDER);
                    strike = double.Parse(parts[4], FORMAT_PROVIDER);
                    return true;
                }
            }

            return dataSource.TryGetAssetType(symbol, out assetClass);
            
        }

        
    }
}
