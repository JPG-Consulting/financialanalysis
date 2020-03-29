using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalyst.Common.Exceptions.EdgarSEC
{
    public class EdgarLineException: ApplicationException
    {
        private int lineNumber;
        private string message;

        public EdgarLineException(string file, int lineNumber, string message) : base(file + " -- " + message)
        {
            this.lineNumber = lineNumber;
            this.message = message;
        }

        public EdgarLineException(string file,int line,Exception inner):base("Error in file " + file + ", line "+ line.ToString(),inner)
        {

        }
    }
}
