using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Prices;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.DataSources
{
    public interface IOptionChainDataSource
    {
        bool TryGetOptionsChain(string ticker, Exchange? exchange, out OptionsChain optionChain, out string errorMessage);
        bool TryGetOptionsChainWithTheoricalValue(string ticker, Exchange? exchange, double lastPrice, out OptionsChain optionsChain, out string errorMessage);
        bool TryGetOptionsChainWithTheoricalValue(string ticker, Exchange? exchange, double lastPrice, PriceList historicalPrices, out OptionsChain optionsChain, out string errorMessage);

        bool TryGetOptionData(string underlyingTicker, ushort year, ushort month, ushort day, out Option option);
    }
}
