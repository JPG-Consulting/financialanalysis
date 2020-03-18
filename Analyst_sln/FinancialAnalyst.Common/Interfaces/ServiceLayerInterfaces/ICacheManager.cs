using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Prices;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces
{
    public interface ICacheManager
    {
        bool TryGetFromCache(string ticker, Exchange? exchange, DateTime? from, DateTime? to, PriceInterval interval, out PriceList prices);
    }
}
