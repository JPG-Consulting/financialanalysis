using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyst.DBAccess.Repositories;
using Analyst.Domain;
using Analyst.Domain.Edgar.Indexes;

namespace Analyst.Services.EdgarServices.EdgarIndexesServices
{
    public interface IEdgarMasterIndexService
    {
        MasterFullIndex ProcessDailyIndex(ushort year, ushort quarter, uint date);
        MasterFullIndex ProcessFullIndex(ushort year, ushort quarter);
        IList<MasterFullIndex> GetAllFullIndexes();
    }


    /// <summary>
    /// Indexes to all public filings are are available from 1994Q3 through the present and located in the following browsable directories:
    /// * /Archives/edgar/daily-index — daily index files through the current year
    /// * /Archives/edgar/full-index — Full indexes offer a "bridge" between quarterly and daily indexes, 
    ///   compiling filings from the beginning of the current quarter through the previous business day.
    ///   At the end of the quarter, the full index is rolled into a static quarterly index.
    /// Note: It doesn't make sense to download daily index from closed quarters because all of them (all daily index for each day of the same quarter) will be included in the full-index
    /// Four types of indexes are available:
    /// * company — sorted by company name
    /// * form — sorted by form type
    /// * master — sorted by CIK number
    /// * XBRL — list of submissions containing XBRL financial files, sorted by CIK number; these include Voluntary Filer Program submissions
    /// The company, form, and master indexes contain the same information sorted differently.
    /// </summary>
    public class EdgarMasterIndexService : IEdgarMasterIndexService
    {
        private IEdgarWebClient webClient;
        private IEdgarFileParser parser;
        private IAnalystEdgarFilesRepository edgarFilesRepo;
        private IAnalystEdgarFilesBulkRepository edgarFilesBulkRepo;
        public EdgarMasterIndexService(IEdgarWebClient webClient, IEdgarFileParser parser,IAnalystEdgarFilesRepository edgarFilesRepository, IAnalystEdgarFilesBulkRepository edgarFilesBulkRepository)
        {
            this.webClient = webClient;
            this.parser = parser;
            this.edgarFilesRepo = edgarFilesRepository;
            this.edgarFilesBulkRepo = edgarFilesBulkRepository;
        }

        public IList<MasterFullIndex> GetAllFullIndexes()
        {
            return edgarFilesRepo.GetFullIndexes();
        }

        public MasterFullIndex ProcessDailyIndex(ushort year, ushort quarter, uint date)
        {
            throw new NotImplementedException();
        }

        public MasterFullIndex ProcessFullIndex(ushort year, ushort quarter)
        {
            MasterFullIndex index;
            Quarter q = (Quarter)quarter;
            
            index = GetFullIndexFromDB(year, q);
            if (index != null && index.IsComplete)
                return index;

            string content;            
            if (GetFullIndexFromWeb(year, q, out content))
            {
                if (index == null)
                {
                    index = new MasterFullIndex();
                    index.Quarter = q;
                    index.Year = year;
                }
                IList<IndexEntry> entries = parser.ParseMasterIndex(content);
                index.TotalLines = entries.Count;
                edgarFilesRepo.Update(index, "TotalLines");
                SaveIndexEntriesToDB(index,entries);
                return index;
            }
            else
            {
                throw new ApplicationException($"It wasn't able to retrieve index (year={year}, quarter={quarter}");
            }
        }

        
        private MasterFullIndex GetFullIndexFromDB(ushort year, Quarter q)
        {
            return edgarFilesRepo.GetFullIndex(year, q);
        }

        private bool GetFullIndexFromWeb(ushort year, Quarter q, out string file)
        {
            bool ret = webClient.DownloadMasterIndex(year, q, out file);
            return ret;
        }

        private void SaveIndexEntriesToDB(MasterFullIndex index, IList<IndexEntry> entries)
        {
            edgarFilesBulkRepo.SaveIndexEntries(index, entries);
            long dbRowsCopied = edgarFilesRepo.GetIndexEntriesCount(index);
            index.ProcessedLines = dbRowsCopied;
            edgarFilesRepo.Update(index, "ProcessedLines");
            if(index.ProcessedLines == index.TotalLines)
            {
                index.IsComplete = true;
                edgarFilesRepo.Update(index, "IsComplete");
            }
        }

        

        


    }
}
