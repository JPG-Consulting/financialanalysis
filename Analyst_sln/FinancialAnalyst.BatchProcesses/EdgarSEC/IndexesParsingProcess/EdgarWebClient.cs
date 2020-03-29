using FinancialAnalyst.BatchProcesses.DB.EdgarSEC.Contexts;
using FinancialAnalyst.Common.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalyst.BatchProcesses.EdgarSEC.IndexesParsingProcess
{
    public interface IEdgarWebClient
    {
        bool DownloadMasterIndex(ushort year, Quarter q, out string content);
        string GetFullIndexUrl(ushort year, Quarter q, string indexType);
    }

    public class EdgarWebClient: IEdgarWebClient
    {

        public static readonly HttpClient httpClient = new HttpClient() { BaseAddress = new Uri(EdgarInitialLoader.SEC_BASE_URL) };

        public EdgarWebClient()
        {

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
