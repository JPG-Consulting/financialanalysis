using FinancialAnalyst.Common.Entities.Assets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Portfolios
{
    [Serializable]
    public class Portfolio
    {
        private List<AssetAllocation> _assetAllocations;

        private List<PortfolioBalance> _balances { get; set; }

        private List<Transaction> _transactions { get; set; }

        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public int UserId { get; set; }

        
        public List<AssetAllocation> AssetAllocations
        {
            get
            {
                if (_assetAllocations == null)
                    _assetAllocations = new List<AssetAllocation>();
                return _assetAllocations;
            }
            set
            {
                _assetAllocations = value;
            }
        }

        public List<PortfolioBalance> Balances
        {
            get
            {
                if (_balances == null)
                    _balances = new List<PortfolioBalance>();
                return _balances;
            }
            set
            {
                _balances = value;
            }
        }

        public List<Transaction> Transactions
        {
            get
            {
                if (_transactions == null)
                    _transactions = new List<Transaction>();
                return _transactions;
            }
            set
            {
                _transactions = value;
            }
        }

        [JsonProperty]
        public decimal TotalCash { get; set; }

        [JsonProperty]
        public string Name { get; set; }
        public decimal InitialBalance { get; set; }

        public static Portfolio From(string name, string[] tickers)
        {
            Portfolio p = new Portfolio()
            {
                Name = name,
                AssetAllocations = new List<AssetAllocation>(),
            };
            foreach(string ticker in tickers)
            {
                p.AssetAllocations.Add(new AssetAllocation()
                {
                    Ticker=ticker,
                });
            }
            return p;
        }
    }
}
