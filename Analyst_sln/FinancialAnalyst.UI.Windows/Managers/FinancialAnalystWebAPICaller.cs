using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Portfolios;
using FinancialAnalyst.Common.Entities.RequestResponse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalyst.UI.Windows.Managers
{
    internal class FinancialAnalystWebAPICaller
    {
#if DEBUG
        //TODO: url has to be an app parameter
        private static readonly HttpClient httpClient = new HttpClient() { BaseAddress = new Uri("http://localhost:51249/api") };
#endif

        static internal IEnumerable<Portfolio> GetPortfoliosByUser(string user)
        {
            string uri = $"{httpClient.BaseAddress}/Portfolios/getportfoliosbyuser";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            WebResponse response = request.GetResponse();
            string jsonResponse;
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                jsonResponse = reader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<IEnumerable<Portfolio>>(jsonResponse);
        }

        static internal APIResponse<Stock> GetAssetData(string ticker, Market market)
        {
            string uri = $"{httpClient.BaseAddress}/DataSources/getassetdata?ticker={ticker}&market={market.ToString()}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            WebResponse response = request.GetResponse();
            string jsonResponse;
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                jsonResponse = reader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<APIResponse<Stock>>(jsonResponse);
        }
    }
}
