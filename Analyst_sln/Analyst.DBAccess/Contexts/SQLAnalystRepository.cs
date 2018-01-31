using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.DBAccess.Contexts
{
    public class SQLAnalystRepository : AnalystRepository
    {
        public SQLAnalystRepository(AnalystContext context) : base(context)
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
            int iSize;
            
            using (SqlConnection conn = CreateBulkConnection())
            {
                conn.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                {
                    bulkCopy.DestinationTableName = "dbo." + tableName;

                    try
                    {
                        if (int.TryParse(strSize, out iSize))
                            bulkCopy.BatchSize = iSize;
                        bulkCopy.WriteToServer(dt);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

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
    }
}
