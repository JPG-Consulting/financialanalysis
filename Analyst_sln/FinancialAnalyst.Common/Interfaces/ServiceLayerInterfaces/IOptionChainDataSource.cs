using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces
{
    public interface IOptionChainDataSource
    {
        bool TryGetOptionsChain(string ticker, Exchange? exchange, out OptionChain optionChain, out string errorMessage);
    }
}
