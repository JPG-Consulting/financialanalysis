using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Services.EdgarServices
{
    public class EdgarTaskState
    {
        public bool? Result { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }

        private EdgarDataset ds;
        public EdgarDataset Dataset { get { return ds; } }
        public EdgarTaskState(EdgarDataset ds)
        {
            this.ds = ds;
        }

    }
}
