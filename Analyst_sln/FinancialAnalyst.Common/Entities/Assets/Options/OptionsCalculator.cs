using FinancialAnalyst.Common.Entities.Prices;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Assets.Options
{
    public class OptionsCalculator
    {
        public const int TRADING_DAYS_PER_YEAR = 252;

        /// <summary>
        /// It uses the Black-Scholes formula to calculate the theorical value of an european option.
        /// 
        /// See:
        ///     Book "Options, futures and other derivatives" from John Hull, 8th edition
        ///     Chapter 14 "The Black-Scholes-Merton model" (page 299)
        /// </summary>
        /// <param name="stock">Underlying asset</param>
        /// <param name="prices">Daily prices of the underlying asset for the last year</param>
        /// <param name="optionChain">Current options chain existing in the market</param>
        /// <param name="riskFreeRate">
        /// When the Black-Scholes formula is used in practice the interest rate r (risk free rate) 
        /// is set equal to the zero-coupon risk-free interest rate for a maturity T
        /// </param>
        public static void CalculateTheoricalValue(PriceList prices, OptionsChain optionChain, RiskFreeRates riskFreeRates, double lastPrice)
        {
            double volatility  = CalculateVolatility(prices);
            CalculateTheoricalValue(prices, optionChain, riskFreeRates, lastPrice, volatility);
        }

        public static void CalculateTheoricalValue(PriceList prices, OptionsChain optionChain, RiskFreeRates riskFreeRates, double lastPrice, double volatility)
        {
            optionChain.HistoricalVolatility = volatility;
            optionChain.Prices = prices;
            optionChain.RiskFreeRates = riskFreeRates;

            foreach (var options in optionChain)
            {
                foreach (Option option in options.Value)
                {
                    if (option.IsCall)
                    {
                        option.TheoricalValue = CalculateCall(lastPrice, volatility, option, riskFreeRates.TwoYears);
                    }
                    else if (option.IsPut)
                    {
                        option.TheoricalValue = CalculatePut(lastPrice, volatility, option, riskFreeRates.TwoYears);
                    }
                    else
                    {
                        //Exotic Option
                        //https://www.investopedia.com/terms/e/exoticoption.asp
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// Formula for common stocks
        ///     c = S0.N(d1) - K.e^(-r.T).N(d2)
        /// 
        ///     d1 = [ ln(S0/K) + (r + σ^2/2).T ] / [ σ.SQRT(T)]
        ///     d2 = d1 - σ.SQRT(T)
        /// 
        ///     where
        ///         c: european call value
        ///         S0: current price of underlying asset (From spanish: Subyacente)
        ///         K: Strike price
        ///         N(x): It is the cumulative probability distribution function for a standardized normal distribution
        ///               It is the probability that a variable with a standard normal distribution Phi(0,1), will be less than x.
        ///         r: Continuously compunded risk-free rate.
        ///         σ: volatility of underlying asset price.
        ///         T: Time to maturity of the option (T= NumberOfTradingDaysUntilOptionMaturity / TradingDaysPerYear)
        /// 
        /// Formula for stocks with dividends:
        ///     c = S0.e^(-d.T).N(d1) - X.e^(r.T).N(d2)
        ///     where
        ///         d = dividend rate (if not, 0)
        ///         
        /// See:
        ///     Book: Options, futures and other derivatives from John Hull, 8th edition
        ///     Seciontion: 14.8 - Black-Scholes-Merton pricing formulas (page 313)
        /// </summary>
        /// <returns></returns>
        public static double CalculateCall(double lastPrice,double volatility, Option option, double riskFreeRate)
        {
            double s0 = lastPrice;
            double σ = volatility;
            double T = (option.ExpirationDate - DateTime.Now).TotalDays / TRADING_DAYS_PER_YEAR;
            double K = option.Strike;
            double d1 = (Math.Log(s0 / K) + (riskFreeRate + Math.Pow(σ,2) / 2) * T) / (σ * Math.Sqrt(T));
            double Nd1 = CumulativeProbabilityNormalDistributionFunction(d1);
            double d2 = d1 - σ * Math.Sqrt(T);
            double Nd2 = CumulativeProbabilityNormalDistributionFunction(d2);
            double c = s0 * Nd1 - K * Math.Pow(Math.E, -riskFreeRate * T) * Nd2;
            return c;
        }

        /// <summary>
        /// Formula for common stocks:
        ///     p = K.e^(-r.T).N(-d2) - S0.N(-d1)
        /// 
        ///     d1 = [ ln(S0/K) + (r + σ^2/2).T ] / [ σ.SQRT(T)]
        ///     d2 = d1 - σ.SQRT(T)
        /// 
        ///     where
        ///         p: european put value
        ///         S0: current price of underlying asset (From spanish: Subyacente)
        ///         K: Strike price
        ///         N(x): It is the cumulative probability distribution function for a standardized normal distribution
        ///               It is the probability that a variable with a standard normal distribution Phi(0,1), will be less than x.
        ///         r: Continuously compunded risk-free rate.
        ///         σ: volatility of underlying asset price.
        ///         T: Time to maturity of the option (T= NumberOfTradingDaysUntilOptionMaturity / TradingDaysPerYear)
        /// 
        /// Formula for stocks with dividends:
        ///     p = K.e^(-r.T).N(-d2) - S0.e^(-d.T).N(-d1)
        ///     where
        ///         d = dividend rate (if not, 0)
        ///         
        /// Source:
        ///     Book: Options, futures and other derivatives from John Hull, 8th edition
        ///     Seciontion: 14.8 - Black-Scholes-Merton pricing formulas (page 313)
        /// </summary>
        /// <returns></returns>
        public static double CalculatePut(double lastPrice, double volatility, Option option, double riskFreeRate)
        {
            double s0 = lastPrice;
            double σ = volatility;
            double T = (option.ExpirationDate - DateTime.Now).TotalDays / TRADING_DAYS_PER_YEAR;
            double K = option.Strike;
            double d1 = (Math.Log(s0 / K) + (riskFreeRate + Math.Pow(σ, 2) / 2) * T) / (σ * Math.Sqrt(T));
            double _Nd1 = CumulativeProbabilityNormalDistributionFunction(-d1);
            double d2 = d1 - σ * Math.Sqrt(T);
            double _Nd2 = CumulativeProbabilityNormalDistributionFunction(-d2);
            double p = K * Math.Pow(Math.E, -riskFreeRate * T) * _Nd2 - s0 * _Nd1;

            return p;
        }


        /// <summary>
        /// The volatility of a stock (sigma symbol) is a measure if our uncertainty about the returns provided by the stock.
        /// The volatility of a stock price can be defined as the standard deviation of the return provided by the stock in 1 year
        /// when the return is expressed using continuous compounding.
        /// 
        /// From:
        ///     Book: Options, futures and other derivatives from John Hull, 8th edition
        ///     Seciontion: 14.4 - Volatility (page 303)
        /// </summary>
        /// <param name="prices"></param>
        /// <returns></returns>
        public static double CalculateVolatility(PriceList prices)
        {
            double meanOf_u = 0;
            for (int i = 0; i < prices.Count; i++)
            {
                meanOf_u += (double)prices[i].Close;
            }
            meanOf_u = meanOf_u / prices.Count;

            double sumOf_U_and_meanOfU_squared = 0;
            double s_prevoius = (double)prices[0].Close;
            for(int i = 1; i <prices.Count; i++)
            {
                double s = (double)prices[i].Close;
                double u_i = Math.Log(s / s_prevoius);
                sumOf_U_and_meanOfU_squared += Math.Pow(u_i - meanOf_u, 2);
                s_prevoius = s;
            }

            int n = prices.Count;
            double standardDeviation_DailyReturn = Math.Sqrt(sumOf_U_and_meanOfU_squared / (n - 1));
            double annualVolatility = standardDeviation_DailyReturn / Math.Sqrt(TRADING_DAYS_PER_YEAR);
            return annualVolatility / 100;

            //otra opcion es usar la 2da formula, solo se evita el loop para calcular el promedio
            //https://es.wikipedia.org/wiki/Desviaci%C3%B3n_t%C3%ADpica#Definici%C3%B3n_de_los_valores_de_una_poblaci%C3%B3n

        }

        private static double CumulativeProbabilityNormalDistributionFunction(double x)
        {
            /*
             * Related links
             * https://stackoverflow.com/questions/1662943/standard-normal-distribution-z-value-function-in-c-sharp
             * https://numerics.mathdotnet.com/
             * https://en.wikipedia.org/wiki/Normal_distribution
             * https://en.wikipedia.org/wiki/Error_function
             */

            MathNet.Numerics.Distributions.Normal result = new MathNet.Numerics.Distributions.Normal();
            return result.CumulativeDistribution(x);
        }
    }
}
