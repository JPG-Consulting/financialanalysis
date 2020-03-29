using FinancialAnalyst.Common.Entities.EdgarSEC.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalyst.BatchProcesses.DB.EdgarSEC.Repositories.BulkRepositories
{

    public interface IEdgarDatasetsBulkRepository : IDisposable
    {
        void DeleteAllRows(int id, DatasetsTables file);
        DataTable GetEmptyDataTable(DatasetsTables relatedTable);
        void BulkCopyTable(DatasetsTables table, DataTable dt);
    }

    public class EdgarDatasetsBulkRepository : BulkRepositoryBase,IEdgarDatasetsBulkRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(EdgarDatasetsBulkRepository));

        protected override log4net.ILog Log { get { return log; } }

        public EdgarDatasetsBulkRepository()
        {

        }
        public DataTable GetEmptyDataTable(DatasetsTables table)
        {
            return GetEmptyDataTable("EdgarDataset" + Enum.GetName(typeof(DatasetsTables), table));
        }

        public void BulkCopyTable(DatasetsTables table,DataTable dt)
        {
            BulkCopy("EdgarDataset" + Enum.GetName(typeof(DatasetsTables), table), dt);
        }

        public void DeleteAllRows(int id, DatasetsTables table)
        {
            using (SqlConnection conn = CreateBulkConnection())
            {
                try
                {
                    conn.Open();
                    DeleteAllRows(id, table,conn);
                    UpdateDatasetStatus(id, table,conn);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void DeleteAllRows(int id, DatasetsTables table, SqlConnection conn)
        {
            SqlCommand comm = new SqlCommand();
            comm.CommandTimeout = BulkTimeout;
            comm.CommandText = "delete from EdgarDataset" + Enum.GetName(typeof(DatasetsTables), table) + " where DatasetId = " + id;
            comm.CommandType = CommandType.Text;
            comm.Connection = conn;
            comm.ExecuteNonQuery();
        }

        private void UpdateDatasetStatus(int id, DatasetsTables table, SqlConnection conn)
        {
            SqlCommand comm = new SqlCommand();
            comm.CommandText = "update EdgarDatasets set Processed" + Enum.GetName(typeof(DatasetsTables), table) + " = 0 where Id = " + id;
            comm.CommandType = CommandType.Text;
            comm.Connection = conn;
            comm.ExecuteNonQuery();
        }

        public void Dispose()
        {
            
        }
    }
}
