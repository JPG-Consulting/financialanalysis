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
        /*
            Four types of indexes are available:
            * company — sorted by company name
            * form — sorted by form type
            * master — sorted by CIK number
            * XBRL — list of submissions containing XBRL financial files, sorted by CIK number; these include Voluntary Filer Program submissions
            The company, form, and master indexes contain the same information sorted differently.
        */

        // Full index url example: https://www.sec.gov/Archives/edgar/full-index/2018/QTR4/master.idx
        // Daily index url example: https://www.sec.gov/Archives/edgar/daily-index/2019/QTR1/master.20190118.idx 


        public const string BASE_URL = "https://www.sec.gov";
        public const string FULL_INDEX_BASE_URL = BASE_URL + "/Archives/edgar/full-index/{0}/{1}/{2}.idx";
        public const string DAILY_INDEX_BASE_URL = BASE_URL + "/Archives/edgar/daily-index/{0}/{1}/{2}.{3}.idx";

        public readonly HttpClient httpClient;

        public EdgarWebClient()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(BASE_URL);
        }

        public bool DownloadMasterIndex(ushort year, Quarter q, out string content)
        {
            string url = string.Format(FULL_INDEX_BASE_URL,year.ToString(),q.ToString(),"master");

            using (HttpResponseMessage response = httpClient.GetAsync(url).Result)
            {
                content = response.Content.ReadAsStringAsync().Result;
                return true;
            }
        }

        public string GetFullIndexUrl(ushort year, Quarter quarter, string indexType)
        {
            return string.Format(FULL_INDEX_BASE_URL, year, quarter, indexType);
        }
    }
}
