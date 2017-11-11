using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Domain.Edgar.Datasets
{
    public class EdgarDatasetException:ApplicationException
    {
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
