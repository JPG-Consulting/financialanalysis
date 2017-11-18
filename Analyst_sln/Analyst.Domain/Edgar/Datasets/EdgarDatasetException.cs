using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Domain.Edgar.Datasets
{
    public class EdgarDatasetException:EdgarException
    {
        private string file;
        public string File { get; }
        public EdgarDatasetException(string file,Exception ex):base("Process file '" + file + "' failed: " + ex.Message,ex)
        {
            this.file = file;
        }

        public EdgarDatasetException(string file, List<Exception> exceptions)
        {
            this.file = file;           
            this.InnerExceptions = exceptions;
        }
    }
}
