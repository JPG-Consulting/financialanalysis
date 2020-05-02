using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Accounting;
using FinancialAnalyst.Common.Entities.Prices;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.DataSources;
using FinancialAnalyst.Common.Entities.Markets;

namespace FinancialAnalyst.DataSources.FinancialModelingPrep
{
    /// <summary>
    /// https://financialmodelingprep.com/developer/docs/
    /// </summary>
    public class FinancialModelingPrepDataSource : IDataSource
    {
        public bool TryGetAssetType(string symbol, out AssetClass assetType)
        {
            throw new NotImplementedException();
        }

        public bool TryGetCompleteStockData(string ticker, Exchange? exchange, bool includeOptionChain, bool includeFinancialData, out Stock stock, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool TryGetCompleteStockData(string ticker, Exchange? exchange, AssetClass assetType, bool includeOptionChain, bool includeFinancialStatements, out Stock stock, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool TryGetFinancialData(string ticker, Exchange? exchange, out FinancialStatements financialData, out string message)
        {
            //https://financialmodelingprep.com/developer/docs/#Company-Financial-Statements
            throw new NotImplementedException();
        }

        public bool TryGetFinancialData(string ticker, string cik, out string message)
        {
            throw new NotImplementedException();
        }

        public bool TryGetIndexData(MarketIndex index, out Dictionary<string, decimal> tickersProportions, out string message)
        {
            throw new NotImplementedException();
        }

        public bool TryGetIndexesData(out string message)
        {
            throw new NotImplementedException();
        }

        public bool TryGetLastPrice(string ticker, Exchange? exchange, out HistoricalPrice lastPrice, out string message)
        {
            throw new NotImplementedException();
        }

        public bool TryGetLastPrice(string ticker, Exchange? exchange, AssetClass assetType, out HistoricalPrice lastPrice, out string message)
        {
            throw new NotImplementedException();
        }

        public bool TryGetOptionsChain(string ticker, Exchange? exchange, out OptionsChain optionChain, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool TryGetOptionsChainWithTheoricalValue(string ticker, Exchange? exchange, double lastPrice, out OptionsChain optionsChain, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool TryGetOptionsChainWithTheoricalValue(string ticker, Exchange? exchange, double lastPrice, PriceList historicalPrices, out OptionsChain optionsChain, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool TryGetHistoricalPrices(string ticker, Exchange? exchange, DateTime? from, DateTime? to, PriceInterval interval, out PriceList prices, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool TryGetStatistics(string ticker, Exchange? exchange, out string message)
        {
            throw new NotImplementedException();
        }

        public bool TryGetStockSummary(string ticker, Exchange? exchange, out Stock stock, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool TryGetStockSummary(string ticker, Exchange? exchange, AssetClass assetType, out Stock stock, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool TryGetCompleteAssetData(string ticker, Exchange? exchange, AssetClass assetClass, bool includeOptionChain, bool includeFinancialStatements, out AssetBase asset, out string errorMessage)
        {
            throw new NotImplementedException();
        }
    }
}
