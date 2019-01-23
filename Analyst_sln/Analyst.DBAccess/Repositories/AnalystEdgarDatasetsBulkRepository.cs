using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyst.Domain.Edgar;

namespace Analyst.DBAccess.Repositories
{
    public enum DatasetsTables
    {
        //Submissions=0,
        //Tags,
        //Dimensions,
        Calculations=3,
        Texts,
        Numbers,
        Renders,
        Presentations
    }

    public interface IAnalystEdgarDatasetsBulkRepository : IDisposable
    {
        DataTable GetEmptyDimensionsDataTable();
        void BulkCopyDimensions(DataTable dt);
        void BulkCopyNumbers(DataTable dt);
        DataTable GetEmptyNumbersDataTable();
        void BulkCopyPresentations(DataTable dt);
        DataTable GetEmptyPresentationDataTable();
        DataTable GetEmptyRenderDataTable();
        void BulkCopyRenders(DataTable dt);
        void DeleteAllRows(int id, DatasetsTables file);
    }

    public class AnalystEdgarDatasetsBulkRepository : BulkRepositoryBase,IAnalystEdgarDatasetsBulkRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override log4net.ILog Log { get { return log; } }

        public AnalystEdgarDatasetsBulkRepository()
        {

        }

        protected override SqlConnection CreateBulkConnection()
        {
            ConnectionStringSettings connSettings = ConfigurationManager.ConnectionStrings["AnalystEdgarDatasets"];
            return new SqlConnection(connSettings.ConnectionString);
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

        public DataTable GetEmptyNumbersDataTable()
        {
            return GetEmptyDataTable("EdgarDatasetNumbers");
        }

        public void BulkCopyNumbers(DataTable dt)
        {
            BulkCopy("EdgarDatasetNumbers", dt);
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
