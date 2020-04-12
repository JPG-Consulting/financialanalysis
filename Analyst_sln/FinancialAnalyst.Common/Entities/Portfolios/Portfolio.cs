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
        public string Name { get; set; }

        [JsonProperty]
        public decimal? InitialBalance { get; set; }

        [JsonProperty]
        public decimal? TotalCosts { get; set; }

        [JsonProperty]
        public decimal? Cash { get; set; }

        [JsonIgnore]
        public decimal? CashPercentage
        {
            get
            {
                if (Cash.HasValue && MarketValue.HasValue)
                {
                    if (MarketValue.Value > 0)
                        return Cash.Value / MarketValue.Value * 100;
                    else
                        return 0;
                }
                else
                    return null;

            }
        }

        [JsonProperty]
        public decimal? MarketValue { get; set; }

        /// <summary>
        /// It is used to know if asset allocations are simulated or are calculated from existing transactions
        /// </summary>
        [JsonProperty]
        public bool IsSimulated { get; set; }


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
