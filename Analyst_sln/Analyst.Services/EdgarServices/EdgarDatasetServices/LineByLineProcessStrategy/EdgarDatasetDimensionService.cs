using System;
using System.Collections.Concurrent;
using Analyst.Domain.Edgar.Datasets;
using Analyst.DBAccess.Contexts;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Analyst.Domain.Edgar;
using System.Data;
using Analyst.Services.EdgarServices.EdgarDatasetServices.Interfaces;
using Analyst.DBAccess.Repositories;

namespace Analyst.Services.EdgarDatasetServices.LineByLineProcessStrategy
{
    public class EdgarDatasetDimensionService : EdgarDatasetBaseService<EdgarDatasetDimension>, IEdgarDatasetDimensionService
    {
        private readonly ILog log;
        protected override ILog Log
        {
            get
            {
                return log;
            }
        }
        public EdgarDatasetDimensionService()
        {
            log = log4net.LogManager.GetLogger(this.GetType().Name);
        }
        public override void Add(IAnalystEdgarDatasetsRepository repo, EdgarDataset dataset, EdgarDatasetDimension file)
        {
            repo.Add(dataset, file);
        }

        public override EdgarDatasetDimension Parse(IAnalystEdgarDatasetsRepository repository, List<string> fieldNames, List<string> fields, int lineNumber)
        {
            string dimhash = fields[fieldNames.IndexOf("dimhash")];
            EdgarDatasetDimension dim;
            dim = new EdgarDatasetDimension();
            dim.DimensionH = dimhash;
            dim.Segments = fields[fieldNames.IndexOf("segments")];
            dim.SegmentTruncated = !(fields[fieldNames.IndexOf("segt")] == "0");
            dim.LineNumber = lineNumber;
            return dim;
        }

        public override IList<EdgarTuple> GetKeys(IAnalystEdgarDatasetsRepository repository, int datasetId)
        {
            return repository.GetDimensionKeys(datasetId);
        }

        public override List<int> GetMissingLinesByTable(IAnalystEdgarDatasetsRepository repo, int datasetId, int totalLines)
        {
            return repo.GetMissingLines(datasetId,"EdgarDatasetDimensions", totalLines);
        }
    }
}