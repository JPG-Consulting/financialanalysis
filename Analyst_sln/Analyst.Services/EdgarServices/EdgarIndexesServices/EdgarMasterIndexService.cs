using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// </summary>
    public class EdgarMasterIndexService : IEdgarMasterIndexService
    {


        private IEdgarWebClient webClient;
        private IEdgarFileParser parser;
        public EdgarMasterIndexService(IEdgarWebClient webClient, IEdgarFileParser parser)
        {
            this.webClient = webClient;
            this.parser = parser;
        }

        public IList<MasterFullIndex> GetAllFullIndexes()
        {
            IList<MasterFullIndex> indexes = new List<MasterFullIndex>();
            for (ushort i = 1993; i <= 2018; i++)
            {
                for (ushort q = (ushort)Quarter.QTR1; q <= (ushort)Quarter.QTR4; q++)
                {
                    MasterFullIndex index = new MasterFullIndex()
                    {
                        Year = i,
                        Quarter = (Quarter)q,
                        RelativeURL = webClient.GetFullIndexUrl(i, (Quarter)q, "master"),
                        IsComplete = false
                    };
                    indexes.Add(index);
                }
            }
            return indexes;
        }

        public MasterFullIndex ProcessDailyIndex(ushort year, ushort quarter, uint date)
        {
            throw new NotImplementedException();
        }

        public MasterFullIndex ProcessFullIndex(ushort year, ushort quarter)
        {
            MasterFullIndex index;
            Quarter q = (Quarter)quarter;
            
            if (GetDailyIndexFromDB(year, q, out index))
                return index;

            String content;
            if (GetDailyIndexFromFileSystemCache(year, q, out content))
            {
                index = new MasterFullIndex();
                index.Quarter = q;
                index.Year = year;
                IList<IndexEntry> entries = parser.ParseMasterIndex(content);
                foreach (IndexEntry entry in entries)
                    index.Entries.Add(entry.CIK, entry);
                SaveDailyIndexToDB(index);
                return index;
            }

            
            if (GetDailyIndexFromWeb(year, q, out content))
            {
                SaveIndexToFileSystemCache(year,q,content);
                index = new MasterFullIndex();
                index.Quarter = q;
                index.Year = year;
                IList<IndexEntry> entries = parser.ParseMasterIndex(content);
                index.Entries = new Dictionary<int, IndexEntry>(entries.Count);
                foreach (IndexEntry entry in entries)
                    index.Entries.Add(entry.CIK, entry);
                SaveDailyIndexToDB(index);
                return index;
            }
            else
            {
                throw new ApplicationException($"It wasn't able to retrieve index (year={year}, quarter={quarter}");
            }
        }

        
        private bool GetDailyIndexFromDB(ushort year, Quarter q, out MasterFullIndex index)
        {
            //TODO: Guardar indice en BD para consulta de reportes
            index = null;
            return false;
        }

        private bool GetDailyIndexFromFileSystemCache(ushort year, Quarter q,  out string content)
        {
            //TODO: Analizar si vale la pena guardarlo en disco para procesarlo despues
            content = null;
            return false;
        }

        private void SaveDailyIndexToDB(MasterFullIndex index)
        {
            //TODO: Implementar el guardado en BD
        }

        private void SaveIndexToFileSystemCache(ushort year, Quarter q, string content)
        {
            //TODO: Analizar si vale la pena guardarlo en disco para procesarlo despues
        }


        private bool GetDailyIndexFromWeb(ushort year, Quarter q,out string file)
        {
            bool ret = webClient.DownloadMasterIndex(year, q,out file);
            return ret;
        }

        


    }
}
