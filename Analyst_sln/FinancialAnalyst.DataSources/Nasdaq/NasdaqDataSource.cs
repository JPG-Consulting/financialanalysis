using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Assets.Options;
using FinancialAnalyst.Common.Entities.Prices;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.DataSources.Nasdaq
{
    public class NasdaqDataSource : IOptionChainDataSource,IStockDataDataSource, IFinancialDataSource
    {
        public bool TryGetStockData(string ticker, Exchange? exchange, out Stock asset, out string errorMessage)
        {
            //https://api.nasdaq.com/api/quote/GM/info?assetclass=stocks
            //GM = General motors

            throw new NotImplementedException();
        }

        public bool TryGetFinancialData(string ticker,Exchange? exchange,out string message)
        {
            //https://api.nasdaq.com/api/company/GM/financials?frequency=1
            //https://api.nasdaq.com/api/company/AMZN/financials?frequency=1
            throw new NotImplementedException();
        }

        public bool TryGetOptionsChain(string ticker, Exchange? exchange, out OptionsChain optionChain, out string errorMessage)
        {
            DateTime from = new DateTime(DateTime.Now.Year,DateTime.Now.Month,1);
            DateTime to = new DateTime(DateTime.Now.Year, DateTime.Now.Month, GetLastDay(DateTime.Now.Month));
            bool ok = NasdaqApiCaller.GetOptionChain(ticker, from, to, out string jsonResponse, out errorMessage);

            optionChain = new OptionsChain();
            dynamic rawdata = JsonConvert.DeserializeObject(jsonResponse);
            
            foreach (var optionRow in rawdata.data.optionChainList.rows)
            {
                if(optionRow.call != null)
                {
                    CallOption o = new CallOption()
                    {
                        Symbol = optionRow.call.symbol,
                        Last = optionRow.call.last,
                        Strike = optionRow.call.strike,
                        ExpirationDate = optionRow.call.expiryDate,
                    };
                    optionChain.Add(o);
                }

                if (optionRow.put != null)
                {
                    PutOption o = new PutOption()
                    {
                        Symbol = optionRow.put.symbol,
                        Last = optionRow.put.last,
                        Strike = optionRow.put.strike,
                        ExpirationDate = optionRow.put.expiryDate,
                    };
                    optionChain.Add(o);
                }
            }

            errorMessage = "ok";
            return true;
        }

        private int GetLastDay(int month)
        {
            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    return 31;
                case 4:
                case 6:
                case 9:
                case 11:
                    return 30;
                case 2:
                    return 28;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
