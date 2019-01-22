using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyst.Domain.Edgar.Indexes;

namespace Analyst.DBAccess.Repositories
{
    public interface IAnalystEdgarFilesBulkRepository
    {
        long SaveIndexEntries(MasterFullIndex index, IList<IndexEntry> entries);
    }

    public class AnalystEdgarFilesBulkRepository: BulkRepositoryBase,IAnalystEdgarFilesBulkRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected override log4net.ILog Log { get { return log; } }

        public long SaveIndexEntries(MasterFullIndex index, IList<IndexEntry> entries)
        {
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
            long rowsCopied = BulkCopy(tableName, dt);
            return rowsCopied;
        }

        protected override SqlConnection CreateBulkConnection()
        {
            ConnectionStringSettings connSettings = ConfigurationManager.ConnectionStrings["AnalystEdgarFiles"];
            return new SqlConnection(connSettings.ConnectionString);
        }
    }
}
