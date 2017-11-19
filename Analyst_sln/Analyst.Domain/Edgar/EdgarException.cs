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
        private List<Exception> innerExceptions;
        public EdgarException()
        {
            innerExceptions = new List<Exception>();
        }

        public EdgarException(string message, Exception inner):base(message,inner)
        {
            innerExceptions = new List<Exception>();
        }

        public List<Exception> InnerExceptions
        {
            get { return innerExceptions; }
            protected set { innerExceptions = value; }
        }
        public void AddInnerException(Exception ex)
        {
            innerExceptions.Add(ex);
        }

    }
}
