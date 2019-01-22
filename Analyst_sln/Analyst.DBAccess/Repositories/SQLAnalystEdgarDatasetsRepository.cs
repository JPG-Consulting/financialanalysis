using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.DBAccess.Repositories
{
    public class SQLAnalystEdgarDatasetsRepository : AnalystEdgarDatasetsRepository, IDisposable
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SQLAnalystEdgarDatasetsRepository():base()
        {

        }

        private DataTable GetEmptyDataTable(string tableName)
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

        private SqlConnection CreateBulkConnection()
        {
            ConnectionStringSettings connSettings = ConfigurationManager.ConnectionStrings["AnalystBulk"];
            return new SqlConnection(connSettings.ConnectionString);
        }

        private void BulkCopy(string tableName, DataTable dt)
        {
            string strSize = ConfigurationManager.AppSettings["bulk_batch_size"];
            string strTimeout = ConfigurationManager.AppSettings["bulk_timeout"];
            int temp;
            log.Info("Table " + tableName + " -- Starting bulk copy process");
            using (SqlConnection conn = CreateBulkConnection())
            {
                conn.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                {
                    log.Info("Table " + tableName + " -- configuring");
                    if (int.TryParse(strTimeout, out temp))
                        bulkCopy.BulkCopyTimeout = temp;
                    else
                        bulkCopy.BulkCopyTimeout = 60 * 60;//1 hour by default
                    
                    if (int.TryParse(strSize, out temp))
                        bulkCopy.BatchSize = temp;
                    else
                        bulkCopy.BatchSize = 10000;//default value
                    bulkCopy.SqlRowsCopied += BulkCopy_SqlRowsCopied;
                    bulkCopy.NotifyAfter = bulkCopy.BatchSize;

                    bulkCopy.DestinationTableName = "dbo." + tableName;

                    try
                    {
                        log.Info("Table " + tableName + " -- Starting bulk copy");
                        bulkCopy.WriteToServer(dt);
                        log.Info("Table " + tableName + " -- bulk copy ended");
                    }
                    catch (Exception ex)
                    {
                        log.Fatal("Table " + tableName + " -- Error: " + ex.Message,ex);
                        throw ex;
                    }
                    finally
                    {
                        conn.Close();
                        log.Info("Table " + tableName + " -- bulk copy process ended");
                    }
                }
            }

        }

        private void BulkCopy_SqlRowsCopied(object sender, SqlRowsCopiedEventArgs e)
        {
            SqlBulkCopy copier = sender as SqlBulkCopy;
            log.Info("Rows copied in " + copier.DestinationTableName + ": " + e.RowsCopied);
        }

        public DataTable GetEmptyDimensionsDataTable()
        {
            return GetEmptyDataTable("EdgarDatasetDimensions");
        }

        public void BulkCopyDimensions(DataTable dt)
        {
            BulkCopy("EdgarDatasetDimensions", dt);
        }

        public DataTable GetEmptyPresentationDataTable()
        {
            return GetEmptyDataTable("EdgarDatasetPresentations");
        }
        public void BulkCopyPresentations(DataTable dt)
        {
            BulkCopy("EdgarDatasetPresentations", dt);
        }
        public DataTable GetEmptyRenderDataTable()
        {
            return GetEmptyDataTable("EdgarDatasetRenders");
        }

        public void BulkCopyRenders(DataTable dt)
        {
            BulkCopy("EdgarDatasetRenders", dt);
        }



        public void BulkCopyNumbers(DataTable dt)
        {
            BulkCopy("EdgarDatasetNumbers", dt);
        }

        public DataTable GetEmptyNumbersDataTable()
        {
            return GetEmptyDataTable("EdgarDatasetNumbers");
        }

    }
}
