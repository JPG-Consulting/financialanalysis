using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.Portfolios;
using FinancialAnalyst.Common.Entities.RequestResponse;
using FinancialAnalyst.Common.Utils;
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
        public static IEnumerable<Portfolio> GetPortfoliosByUser(string username)
        {
            string uri = $"api/Portfolios/getportfoliosbyuser?username={username}";
            HttpStatusCode statusCode = HttpClientWebAPI.Get(uri, out string jsonResponse);
            JsonConverter[] converters = { new AssetsJsonConverter() };
            APIResponse<IEnumerable<Portfolio>> response = JsonConvert.DeserializeObject<APIResponse<IEnumerable<Portfolio>>>(jsonResponse, new JsonSerializerSettings() { Converters = converters });
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

        public static bool Save(int id, decimal marketValue, out APIResponse<bool> response, out string message)
        {
            string uri = $"api/Portfolios/updateportfolio";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("id", id.ToString());
            parameters.Add("marketValue", marketValue.ToString());

            HttpStatusCode httpStatusCode = HttpClientWebAPI.Post(uri, parameters, out string jsonResponse, out string reasonPhrase); ;
            response = JsonConvert.DeserializeObject<APIResponse<bool>>(jsonResponse);
            if (httpStatusCode == HttpStatusCode.OK)
            {
                
                message = response.ErrorMessage;
                return response.Ok;
            }
            else
            {
                message = $"{reasonPhrase} - {response.ErrorMessage}";
                return false;
            }
        }

        public static bool UpdateAssetAllocation(AssetAllocation assetAllocation, out APIResponse<AssetAllocation> response, out string message)
        {
            string uri = $"api/Portfolios/updateassetallocation";
            HttpStatusCode httpStatusCode = HttpClientWebAPI.Post<int>(uri, assetAllocation.Id, out string jsonResponse, out string reasonPhrase);
            if (httpStatusCode == HttpStatusCode.OK)
            {
                response = JsonConvert.DeserializeObject<APIResponse<AssetAllocation>>(jsonResponse);
                message = response.ErrorMessage;
                return response.Ok;
            }
            else
            {
                response = null;
                message = reasonPhrase;
                return false;
            }
        }
    }
}
