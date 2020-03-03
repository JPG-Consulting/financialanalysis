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
            //return GetSummary_v2(ticker, out jsonResponse, out errorMessage);
            return GetSummary_v1(ticker, out jsonResponse, out errorMessage);
        }

        internal static bool GetSummary_v1(string ticker, out string jsonResponse, out string errorMessage)
        {
            //https://stackoverflow.com/questions/20990601/decompressing-gzip-stream-from-httpclient-response

            httpClientSummary.DefaultRequestHeaders.Clear();
            //httpClientSummary.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("PostmanRuntime/7.22.0")));
            httpClientSummary.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            //httpClientSummary.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue();
            //httpClientSummary.DefaultRequestHeaders.CacheControl.NoCache = true;
            //httpClientSummary.DefaultRequestHeaders.Postman-Token
            //httpClientSummary.DefaultRequestHeaders.Host = "www.reuters.com";
            httpClientSummary.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            httpClientSummary.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            httpClientSummary.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("br"));
            //httpClientSummary.DefaultRequestHeaders.Cookie
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

        internal static bool GetSummary_v2(string ticker, out string jsonResponse, out string errorMessage)
        {
            //https://stackoverflow.com/questions/36939980/strange-httpclient-result

            string uri = $"{httpClientSummary.BaseAddress}{ticker}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.KeepAlive = false;
            request.ContentType = "application/json; charset=utf-8";
            request.AutomaticDecompression = DecompressionMethods.GZip;
            WebResponse response = request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                jsonResponse = reader.ReadToEnd();
            }
            errorMessage = "ok";
            return true;
        }
    }
}
