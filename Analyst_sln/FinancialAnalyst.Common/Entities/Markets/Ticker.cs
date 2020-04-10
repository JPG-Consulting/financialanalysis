using FinancialAnalyst.Common.Entities.EdgarSEC;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Markets
{
    public class Ticker
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }

        public Registrant Registrant { get; set; }

    }
}
