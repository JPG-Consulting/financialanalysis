using FinancialAnalyst.Common.Entities.EdgarSEC.Indexes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalyst.DataAccess.EdgarSEC.Repositories.BulkRepositories
{
    public interface IEdgarFilesBulkRepository
    {
        long SaveIndexEntries(MasterIndex index, IList<IndexEntry> entries);
    }

    public class EdgarFilesBulkRepository: BulkRepositoryBase,IEdgarFilesBulkRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected override log4net.ILog Log { get { return log; } }

        public long SaveIndexEntries(MasterIndex index, IList<IndexEntry> entries)
        {
            log.Info("SaveIndexEntries - Saving data to datatable");
            string tableName = "IndexEntries";
            DataTable dt = GetEmptyDataTable(tableName);
            foreach(IndexEntry entry in entries)
            {
                DataRow dr = dt.NewRow();
                dr["CIK"] = entry.CIK;
                dr["FormTypeId"] = entry.FormTypeId;
                dr["DateFiled"] = entry.DateFiled;
                dr["RelativeURL"] = entry.RelativeURL;
                dr["MasterIndexId"] = index.Id;
                //dr["MasterDailyIndex_Id"] = ?????;
                dt.Rows.Add(dr);
            }
            log.Info($"SaveIndexEntries - Datatable has {dt.Rows.Count} rows, starting bulk copy");
            long rowsCopied = BulkCopy(tableName, dt);
            log.Info($"SaveIndexEntries - Bulk copy ended ok");
            return rowsCopied;
        }
    }
}
