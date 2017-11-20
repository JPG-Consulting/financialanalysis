using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Domain.Edgar.Exceptions
{
    public class EdgarLineException: ApplicationException
    {
        public EdgarLineException(string file,int line,Exception inner):base("Error in file " + file + ", line "+ line.ToString(),inner)
        {

        }
    }
}
