using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace FinancialAnalyst.DataSources.Reuters
{

    internal class ReutersApiCaller
    {
        private static readonly HttpClientHandler handler = new HttpClientHandler()
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };
        private static readonly HttpClient httpClientSummary = new HttpClient(handler) { BaseAddress = new Uri("https://www.reuters.com/companies/api/getFetchCompanyProfile/") };
        private static readonly HttpClient httpClientFinancials = new HttpClient(handler) { BaseAddress = new Uri("https://www.reuters.com/companies/api/getFetchCompanyFinancials/") };
        
        //private static readonly HttpClient httpClientKeyMetrics = new HttpClient(handler) { BaseAddress = new Uri("https://www.reuters.com/companies/api/getFetchCompanyKeyMetrics/") };

        internal static bool GetSummary(string ticker, out string jsonResponse, out string errorMessage)
        {
            string uri = $"{httpClientSummary.BaseAddress}{ticker}";
            return Get(httpClientSummary, uri, out jsonResponse, out errorMessage);
            
        }

        internal static bool GetFinancialData(string ticker, out string jsonResponse, out string errorMessage)
        {
            string uri = $"{httpClientFinancials.BaseAddress}{ticker}";
            return Get(httpClientFinancials, uri, out jsonResponse, out errorMessage);
        }

        private static bool Get(HttpClient client, string uri, out string jsonResponse, out string errorMessage)
        {
            //https://stackoverflow.com/questions/20990601/decompressing-gzip-stream-from-httpclient-response

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("br"));
            client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
            client.DefaultRequestHeaders.Connection.Add("keep-alive");

            HttpResponseMessage responseMessage = client.GetAsync(uri).Result;
            string originalContent = responseMessage.Content.ReadAsStringAsync().Result;
            string content = originalContent;
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                errorMessage = "OK";
                jsonResponse = content;
                return true;
            }
            else
            {
                dynamic error = new
                {
                    HttpStatusCode = responseMessage.StatusCode.ToString(),
                    ReasonPhrase = responseMessage.ReasonPhrase,
                    ContentResponse = content,
                };
                jsonResponse = JsonConvert.SerializeObject(error);
                errorMessage = responseMessage.ReasonPhrase;
                return false;
            }
        }
    }
}
