using Analyst.DBAccess.Contexts;
using Analyst.Domain;
using Analyst.Domain.Edgar.Indexes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Services.EdgarServices.EdgarIndexesServices
{
    public interface IEdgarWebClient
    {
        bool DownloadMasterIndex(ushort year, Quarter q, out string content);
        string GetFullIndexUrl(ushort year, Quarter q, string indexType);
    }

    public class EdgarWebClient: IEdgarWebClient
    {
       
        public readonly HttpClient httpClient;

        public EdgarWebClient()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(EdgarInitialLoader.SEC_BASE_URL);
        }

        public bool DownloadMasterIndex(ushort year, Quarter q, out string content)
        {
            string url = string.Format(EdgarInitialLoader.FULL_INDEX_BASE_URL,year.ToString(),q.ToString(),"master");

            using (HttpResponseMessage response = httpClient.GetAsync(url).Result)
            {
                content = response.Content.ReadAsStringAsync().Result;
                return true;
            }
        }

        public string GetFullIndexUrl(ushort year, Quarter quarter, string indexType)
        {
            return EdgarInitialLoader.GetFullIndexUrl(year, quarter, indexType);
        }
    }
}
