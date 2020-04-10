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

        /// <summary>
        /// It creates a portfolio from the transactions.
        /// First line of fileStream has to be the initial balance.
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="portfolioname"></param>
        /// <param name="fileStream">It expects a CSV file. First line has to be the initial balance</param>
        /// <returns></returns>
        public bool Create(string userName, string portfolioname, Stream fileStream, bool firstRowIsInitalBalance,out Portfolio portfolio, out string message)
        {
            List<string[]> transactionsList = new List<string[]>();

            User user = portfoliosContext.GetUser(userName);
            if(user == null)
            {
                portfolio = null;
                message = $"User '{userName}' doesn't exist.";
                return false;
            }

            portfolio = new Portfolio()
            {
                UserId = user.Id,
                Name = portfolioname,
            };
            int portfolioId = portfoliosContext.Add(portfolio);

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
                        pb.PortfolioId = portfolioId;
                        portfoliosContext.Add(pb);
                        portfolio.Balances.Add(pb);
                        portfolio.InitialBalance = pb.NetCashBalance;
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
                    Transaction t = Transaction.From(portfolioId, fields);
                    
                    if (t != null)
                    {
                        t.UserId = user.Id;
                        t.PortfolioId = portfolioId;
                        portfoliosContext.Add(t);
                        portfolio.Transactions.Add(t);
                    }
                    else
                    {
                        //TODO: Inform to user that transaction wasn't saved
                    }
                }
            }

            AddAssetAllocations(portfolio);

            message = $"Portfilio '{portfolioname}' created successfuly.";
            return true;
        }

        private void AddAssetAllocations(Portfolio portfolio)
        {

            var query = from transaction in portfolio.Transactions
                        group transaction by transaction.Symbol into transactionsGroup
                        select new
                        {
                            Symbol = transactionsGroup.Key,
                            Amount = (from assetAlloc in transactionsGroup select assetAlloc.Amount)
                        };
            
            foreach(var assetAllocationEntry in query)
            {
                AssetAllocation assetAllocation = new AssetAllocation();
                assetAllocation.Ticker = assetAllocationEntry.Symbol;
                assetAllocation.Amount = assetAllocationEntry.Amount.Sum();
                portfolio.AssetAllocations.Add(assetAllocation);
            }

            decimal initialAmount;
            if (portfolio.InitialBalance == 0)
                initialAmount = portfolio.AssetAllocations.Sum(aa => aa.Amount.HasValue ? aa.Amount.Value : 0);
            else
                initialAmount = portfolio.InitialBalance;

            portfolio.TotalCash = initialAmount;
            foreach (AssetAllocation assetAllocation in portfolio.AssetAllocations)
            {
                decimal allocationAmount = assetAllocation.Amount.HasValue ? assetAllocation.Amount.Value : 0;
                assetAllocation.Percentage = allocationAmount / initialAmount;
                portfolio.TotalCash -= allocationAmount;
            }
        }
    }
}
