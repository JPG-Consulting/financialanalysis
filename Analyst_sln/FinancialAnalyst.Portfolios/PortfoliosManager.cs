using FinancialAnalyst.Common.Entities.Portfolios;
using FinancialAnalyst.Common.Entities.Users;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces;
using FinancialAnalyst.DataAccess.Portfolios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace FinancialAnalyst.Portfolios
{
    public class PortfoliosManager : IPortfoliosManager
    {
        private readonly IPortfoliosContext portfoliosContext;
        public PortfoliosManager(IPortfoliosContext portfoliosContext)
        {
            this.portfoliosContext = portfoliosContext;
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
        public bool Create(string userName, string portfolioname, Stream fileStream, bool firstRowIsInitalBalance, bool overrideIfExists, out Portfolio portfolio, out string message)
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
                portfolio = new Portfolio()
                {
                    UserId = user.Id,
                    Name = portfolioname,
                };
                portfolioId = portfoliosContext.Add(portfolio);
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
                            portfoliosContext.Add(pb);//And it also adds the balance to the balances collection of the portfolio
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
                    Transaction newTransaction = Transaction.From(portfolioId, fields);
                    
                    if (newTransaction != null && portfolio.Transactions.Where(t => t.TransactionCode == newTransaction.TransactionCode).Any() == false)
                    {
                        newTransaction.UserId = user.Id;
                        newTransaction.PortfolioId = portfolioId;
                        portfoliosContext.Add(newTransaction);//And it also adds the transaction to the transactions collection of the portfolio
                    }
                    else
                    {
                        //TODO: Inform to user that transaction wasn't saved
                    }
                }
            }

            CalculateAssetAllocations(portfolio);

            message = $"Portfilio '{portfolioname}' created successfuly.";
            return true;
        }

        private void CalculateAssetAllocations(Portfolio portfolio)
        {
            decimal initialAmount;
            if (portfolio.Balances.Count() == 0)
                initialAmount = portfolio.Transactions.Sum(t => t.Amount);
            else
                initialAmount = portfolio.InitialBalance;

            decimal cash = initialAmount;
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
                            Ticker = t.Symbol,
                            Amount = 0,
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
                        assetAllocation.Amount += Math.Abs(t.Amount);
                    }
                    else
                    {
                        assetAllocation.Amount -= Math.Abs(t.Amount);
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

            portfolio.TotalCash = cash;

            foreach(var assetAllocation in groups)
            {
                assetAllocation.Value.Percentage = assetAllocation.Value.Amount / initialAmount * 100;
                portfolio.AssetAllocations.Add(assetAllocation.Value);
            }

            AssetAllocation cashAllocation = new AssetAllocation();
            cashAllocation.PortfolioId = portfolio.Id;
            cashAllocation.Ticker = "Cash";
            cashAllocation.Amount = portfolio.TotalCash;
            cashAllocation.Percentage = portfolio.TotalCash / initialAmount;
            portfolio.AssetAllocations.Add(cashAllocation);

        }
    }
}
