using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Prices
{
    public class PriceList:List<HistoricalPrice>
    {
        public double CalculateStandardDeviation()
        {
            //Volatility is a measure of stock price fluctuation. 
            //Mathematically, volatility is the annualized standard deviation of a stock's daily price changes.

            throw new NotImplementedException();
        }

        public double CalculateBeta(PriceList indexPrices)
        {
            //Beta is the measure of a security's volatility in relation to the S&P 500. Beta less than 1 means the security's price or NAV has been less volatile than the market. 
            //Beta greater than 1 means the security's price or NAV has been more volatile than the market. 

            throw new NotImplementedException();
        }
    }
}
