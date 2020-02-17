using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyst.DBAccess.Contexts;
using Analyst.DBAccess.Repositories;

namespace Analyst.Services.EdgarDatasetServices
{
    public class EdgarTaskState
    {
        public bool? ResultOk { get; set; }
        public string Message { get; set; }
        public EdgarDatasetException Exception { get; set; }

        private EdgarDataset ds;
        public EdgarDataset Dataset { get { return ds; } }

        private IAnalystEdgarDatasetsRepository dsRepo;
        public IAnalystEdgarDatasetsRepository DatasetSharedRepo { get { return dsRepo; } }

        private string processName;
        public string ProcessName { get { return processName; } }

        public string FileNameToReprocess { get; set; }

        public EdgarTaskState(string processName ,EdgarDataset ds,IAnalystEdgarDatasetsRepository dsRepo)
        {
            this.ds = ds;
            this.dsRepo = dsRepo;
            this.processName = processName;
        }

    }
}
