using FinancialAnalyst.Common.Entities.Portfolios;
using FinancialAnalyst.Common.Entities.RequestResponse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace FinancialAnalyst.WebAPICallers
{
    public class PortfoliosAPICaller
    {
        public static IEnumerable<Portfolio> GetDefaultPortfolios()
        {
            string uri = $"api/Portfolios/getdefaultportfolios";
            HttpStatusCode statusCode = HttpClientWebAPI.Get(uri, out string jsonResponse);
            APIResponse<IEnumerable<Portfolio>> response = JsonConvert.DeserializeObject<APIResponse<IEnumerable<Portfolio>>>(jsonResponse);
            return response.Content;
        }

        public static IEnumerable<Portfolio> GetPortfoliosByUser(string user)
        {
            string uri = $"api/Portfolios/getportfoliosbyuser";
            HttpStatusCode statusCode = HttpClientWebAPI.Get(uri, out string jsonResponse);
            APIResponse<IEnumerable<Portfolio>> response = JsonConvert.DeserializeObject<APIResponse<IEnumerable<Portfolio>>>(jsonResponse);
            return response.Content;
        }

        public static APIResponse<Portfolio> CreateNewPortfolioFromTransacions(string username, string portfolioName, Stream file)
        {
            string uri = $"api/Portfolios/createportfolio";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("username", username);
            parameters.Add("portfolioname", portfolioName);
            string name = "transactions";
            string filename = name + ".csv";
            HttpStatusCode statusCode = HttpClientWebAPI.Post(uri, parameters, file,name, filename, out string jsonResponse);
            APIResponse<Portfolio> response = JsonConvert.DeserializeObject<APIResponse<Portfolio>>(jsonResponse);
            return response;
        }
    }
}
