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
    public abstract class BulkRepositoryBase
    {
        protected abstract log4net.ILog Log { get; }

        private long rowsCopied;

        protected DataTable GetEmptyDataTable(string tableName)
        {
            using (SqlConnection conn = CreateBulkConnection())
            {
                SqlCommand comm = new SqlCommand();
                comm.CommandText = "select * from dbo." + tableName + " where 1=2";
                comm.Connection = conn;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = comm;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds.Tables[0];
            }
        }

        protected int BulkTimeout
        {
            get
            {
                string strTimeout = ConfigurationManager.AppSettings["bulk_timeout"];
                int temp;
                if (int.TryParse(strTimeout, out temp))
                    return temp;
                else
                    return 60 * 60;//1 hour by default
            }
        }

        protected int BulkBatchSize
        {
            get
            {
                string strSize = ConfigurationManager.AppSettings["bulk_batch_size"];
                if (int.TryParse(strSize, out int temp))
                    return temp;
                else
                    return 10000;//default value
            }
        }

        protected long BulkCopy(string tableName, DataTable dt)
        {
            
            
            
            Log.Info("Table " + tableName + " -- Starting bulk copy process");
            using (SqlConnection conn = CreateBulkConnection())
            {
                conn.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                {
                    Log.Info("Table " + tableName + " -- configuring");

                    bulkCopy.BulkCopyTimeout = BulkTimeout;
                    bulkCopy.BatchSize = BulkBatchSize;
                    bulkCopy.SqlRowsCopied += BulkCopy_SqlRowsCopied;
                    bulkCopy.NotifyAfter = bulkCopy.BatchSize;

                    bulkCopy.DestinationTableName = "dbo." + tableName;
                    rowsCopied = 0;
                    try
                    {
                        Log.Info("Table " + tableName + " -- Starting bulk copy");
                        bulkCopy.WriteToServer(dt);
                        Log.Info("Table " + tableName + " -- bulk copy ended");
                        return rowsCopied;
                    }
                    catch (Exception ex)
                    {
                        Log.Fatal("Table " + tableName + " -- Error: " + ex.Message, ex);
                        throw ex;
                    }
                    finally
                    {
                        conn.Close();
                        Log.Info("Table " + tableName + " -- bulk copy process ended");
                    }
                }
            }

        }

        protected void BulkCopy_SqlRowsCopied(object sender, SqlRowsCopiedEventArgs e)
        {
            SqlBulkCopy copier = sender as SqlBulkCopy;
            rowsCopied += e.RowsCopied;
            Log.Info("Rows copied in " + copier.DestinationTableName + ": " + e.RowsCopied);
        }

        protected SqlConnection CreateBulkConnection()
        {
            ConnectionStringSettings connSettings = ConfigurationManager.ConnectionStrings["AnalystEdgar"];
            return new SqlConnection(connSettings.ConnectionString);
        }
    }
}
