using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace FinancialAnalyst.DataSources.Reuters
{

    internal class ReutersWebAPI
    {
        private static readonly HttpClientHandler handler = new HttpClientHandler()
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };
        private static readonly HttpClient httpClientSummary = new HttpClient(handler) { BaseAddress = new Uri("https://www.reuters.com/companies/api/getFetchCompanyProfile/") };
        private static readonly HttpClient httpClientFinancials = new HttpClient(handler) { BaseAddress = new Uri("https://www.reuters.com/companies/api/getFetchCompanyFinancials/") };
        private static readonly HttpClient httpClientKeyMetrics = new HttpClient(handler) { BaseAddress = new Uri("https://www.reuters.com/companies/api/getFetchCompanyKeyMetrics/") };

        internal static bool GetSummary(string ticker, out string jsonResponse, out string errorMessage)
        {
            //https://stackoverflow.com/questions/20990601/decompressing-gzip-stream-from-httpclient-response

            httpClientSummary.DefaultRequestHeaders.Clear();
            httpClientSummary.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            httpClientSummary.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            httpClientSummary.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            httpClientSummary.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("br"));
            httpClientSummary.DefaultRequestHeaders.Connection.Add("keep-alive");

            httpClientSummary.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
            httpClientSummary.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));

            string uri = $"{httpClientSummary.BaseAddress}{ticker}";
            HttpResponseMessage responseMessage = httpClientSummary.GetAsync(uri).Result;
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
