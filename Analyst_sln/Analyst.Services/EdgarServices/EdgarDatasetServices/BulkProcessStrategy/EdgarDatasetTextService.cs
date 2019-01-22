using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyst.DBAccess.Contexts;
using System.Collections.Concurrent;
using System.Globalization;
using log4net;
using Analyst.Domain.Edgar;
using System.Data;
using Analyst.Services.EdgarServices.EdgarDatasetServices.Interfaces;
using Analyst.DBAccess.Repositories;

namespace Analyst.Services.EdgarDatasetServices.BulkProcessStrategy
{
    public class EdgarDatasetTextService : EdgarDatasetBaseService<EdgarDatasetText>, IEdgarDatasetTextService
    {
        public ConcurrentDictionary<string, int> Submissions { get; set; }
        public ConcurrentDictionary<string, int> Tags { get; set; }

        public ConcurrentDictionary<string, int> Dimensions { get; set; }

        private readonly ILog log;
        protected override ILog Log
        {
            get
            {
                return log;
            }
        }
        public EdgarDatasetTextService()
        {
            log = log4net.LogManager.GetLogger(this.GetType().Name);
        }


        public override IList<EdgarTuple> GetKeys(IAnalystEdgarDatasetsRepository repository, int datasetId)
        {
            return repository.GetTextKeys(datasetId);
        }

        public override void Parse(List<string> fieldNames, List<string> fields, int lineNumber, DataRow dr, int edgarDatasetId)
        {
            throw new NotImplementedException();
        }

        public override void BulkCopy(IAnalystEdgarDatasetsBulkRepository repo, DataTable dt)
        {
            throw new NotImplementedException();
        }

        public override DataTable GetEmptyDataTable(IAnalystEdgarDatasetsBulkRepository repo)
        {
            throw new NotImplementedException();
        }

        public override List<int> GetMissingLinesByTable(IAnalystEdgarDatasetsRepository repo, int datasetId, int totalLines)
        {
            return repo.GetMissingLines(datasetId,"EdgarDatasetTexts", totalLines);
        }
    }

}
