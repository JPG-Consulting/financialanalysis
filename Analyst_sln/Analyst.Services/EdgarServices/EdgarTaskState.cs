using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyst.DBAccess.Contexts;

namespace Analyst.Services.EdgarServices
{
    public class EdgarTaskState
    {
        public bool? Result { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }

        private EdgarDataset ds;
        public EdgarDataset Dataset { get { return ds; } }

        private IAnalystRepository dsRepo;
        public IAnalystRepository DatasetSharedRepo { get { return dsRepo; } }

        public EdgarTaskState(EdgarDataset ds,IAnalystRepository dsRepo)
        {
            this.ds = ds;
            this.dsRepo = dsRepo;
        }

    }
}
