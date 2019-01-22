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

        public void Dispose()
        {
            
        }
    }
}
