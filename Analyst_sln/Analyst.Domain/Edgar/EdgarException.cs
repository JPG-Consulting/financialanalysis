using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyst.Domain.Edgar.Datasets;

namespace Analyst.Domain.Edgar
{
    public class EdgarException : ApplicationException
    {
        public EdgarException()
        {

        }

        public EdgarException(string message, Exception inner):base(message,inner)
        {

        }
        private List<Exception> innerExceptions = new List<Exception>();
        public List<Exception> InnerExceptions
        {
            get { return innerExceptions; }
        }
        public void AddInnerException(Exception ex)
        {
            innerExceptions.Add(ex);
        }
    }
}
