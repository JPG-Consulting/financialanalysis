using FinancialAnalyst.Common.Entities.EdgarSEC.Datasets;
using FinancialAnalyst.Common.Entities.EdgarSEC.Repositories;
using FinancialAnalyst.Common.Exceptions.EdgarSEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess
{
    public class EdgarTaskState
    {
        public bool? ResultOk { get; set; }
        public string Message { get; set; }
        public EdgarDatasetException Exception { get; set; }

        private EdgarDataset ds;
        public EdgarDataset Dataset { get { return ds; } }

        private IEdgarDatasetsRepository dsRepo;
        public IEdgarDatasetsRepository DatasetSharedRepo { get { return dsRepo; } }

        private string processName;
        public string ProcessName { get { return processName; } }

        public string FileNameToReprocess { get; set; }

        public EdgarTaskState(string processName ,EdgarDataset ds,IEdgarDatasetsRepository dsRepo)
        {
            this.ds = ds;
            this.dsRepo = dsRepo;
            this.processName = processName;
        }

    }
}
