using log4net;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;

namespace FinancialAnalyst.WebAPICallers
{
    public class HttpClientWebAPI
    {
        private static readonly ILog logger = log4net.LogManager.GetLogger(typeof(HttpClientWebAPI));

        private static Uri baseAddress;
        private static readonly object sync = new object();
        private static HttpClient httpClient;

        public static void Initialize(string uriString)
        {
            lock(sync)
            {
                if(httpClient == null)
                {
                    baseAddress = new Uri(uriString);
                    httpClient = new HttpClient() { BaseAddress = baseAddress };
                }
                else
                {
                    logger.Info($"HttpClientWrapper is already initialized, base address: {baseAddress}");
                }
            }
        }

        internal static HttpStatusCode Post(string uri,Dictionary<string,string> parameters, Stream file, string name, string filename, out string jsonResponse)
        {
            byte[] byteContent;
            using (MemoryStream ms = new MemoryStream())
            {
                file.CopyTo(ms);
                byteContent = ms.ToArray();
            }
            ByteArrayContent fileContent = new ByteArrayContent(byteContent);
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(fileContent, name, filename);
            foreach(var parameter in parameters)
            {
                form.Add(new StringContent(parameter.Value), parameter.Key);
            }
            HttpResponseMessage postResponse = httpClient.PostAsync(uri, form).Result;
            jsonResponse = postResponse.Content.ReadAsStringAsync().Result;
            return postResponse.StatusCode;
        }

        internal static HttpStatusCode Get(string uri, out string jsonResponse)
        {
            HttpResponseMessage responseMessage = httpClient.GetAsync(uri).Result;
            jsonResponse = responseMessage.Content.ReadAsStringAsync().Result;
            return responseMessage.StatusCode;
        }
    }
}
