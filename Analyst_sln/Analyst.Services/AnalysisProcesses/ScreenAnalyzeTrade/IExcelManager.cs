using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Services.AnalysisProcesses.ScreenAnalyzeTrade
{
    public interface IExcelManager
    {
        DataTable ReadExcel(Stream inputStream);
    }
}
