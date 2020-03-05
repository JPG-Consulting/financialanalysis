using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace FinancialAnalyst.DataSources.Yahoo.TrialsNotWorking
{
    class Caller
    {
        public AssetBase GetAssetData(string ticker, Exchange market)
        {
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //opcion 1: YahooStockEngine
            //Quote quote = new Quote(ticker);
            //YahooStockEngine.Fetch(quote);
            //return YahooStockEngine.Convert(quote);

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //opcion 2: oath
            //string yql = "select * from search.web where query='pizza'";
            //string yql = "select* from yahoo.finance.quotes where symbol in ('YHOO', 'AAPL', 'GOOG', 'MSFT')";

            //var xml = QueryYahoo(yql);
            //Console.Write(xml.InnerText);

            //string consumerKey = "-- YOUR CONSUMER KEY--";
            //string consumerSecret = "-- YOUR CONSUMER SECRET--";


            //string yql2 = yql + "&env=http%3A%2F%2Fdatatables.org%2Falltables.env";
            //xml = QueryYahoo(yql2, consumerKey, consumerSecret);
            //Console.Write(xml.InnerText);

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Opcion 3: yahoo managed

            return null;
        }

        private static XmlDocument QueryYahoo(string yql)
        {
            string url = "http://query.yahooapis.com/v1/public/yql?format=xml&diagnostics=false&q=" + Uri.EscapeUriString(yql);

            var req = System.Net.HttpWebRequest.Create(url);
            var xml = new XmlDocument();
            using (var res = req.GetResponse().GetResponseStream())
            {
                xml.Load(res);
            }
            return xml;
        }

        private static XmlDocument QueryYahoo(string yql, string consumerKey, string consumerSecret)
        {
            string url = "http://query.yahooapis.com/v1/yql?format=xml&diagnostics=false&q=" + Uri.EscapeUriString(yql);
            url = OAuth.GetUrl(url, consumerKey, consumerSecret);

            var req = System.Net.HttpWebRequest.Create(url);
            var xml = new XmlDocument();
            using (var res = req.GetResponse().GetResponseStream())
            {
                xml.Load(res);
            }
            return xml;
        }
    }
}
