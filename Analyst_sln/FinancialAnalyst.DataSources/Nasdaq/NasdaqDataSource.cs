using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Prices;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.DataSources.Nasdaq
{
    public class NasdaqDataSource : IOptionChainDataSource,IAssetDataDataSource
    {
        public bool TryGetAssetData(string ticker, Exchange? exchange, out AssetBase asset, out string errorMessage)
        {
            //https://api.nasdaq.com/api/quote/GM/info?assetclass=stocks
            //GM = General motors

            throw new NotImplementedException();
        }

        public bool TryGetOptionsChain(string ticker, Exchange? exchange, out OptionChain optionChain, out string errorMessage)
        {
            DateTime from = new DateTime(DateTime.Now.Year,DateTime.Now.Month,1);
            DateTime to = new DateTime(DateTime.Now.Year, DateTime.Now.Month, GetLastDay(DateTime.Now.Month));
            bool ok = NasdaqApiCaller.GetOptionChain(ticker, from, to, out string jsonResponse, out errorMessage);

            optionChain = new OptionChain();
            errorMessage = "Pending to parse data";
            return false;
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
