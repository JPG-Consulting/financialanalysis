using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Services.AnalysisProcesses.ScreenAnalyzeTrade
{
    public class ExcelManager : IExcelManager
    {
        public DataTable ReadExcel(Stream inputStream)
        {
            using (var reader = ExcelReaderFactory.CreateReader(inputStream))
            {

                DataSet ds = reader.AsDataSet();
                return ds.Tables[0];
                
            }
        }
    }
}
