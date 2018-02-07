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

namespace Analyst.Services.EdgarDatasetServices
{
    public interface IEdgarDatasetDimensionService : IEdgarDatasetBaseService<EdgarDatasetDimension>
    {
        
    }
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
        public override void Add(IAnalystRepository repo, EdgarDataset dataset, EdgarDatasetDimension file)
        {
            repo.Add(dataset, file);
        }

        public override EdgarDatasetDimension Parse(IAnalystRepository repository, List<string> fieldNames, List<string> fields, int lineNumber)
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

        public override IList<EdgarTuple> GetKeys(IAnalystRepository repository, int datasetId)
        {
            return repository.GetDimensionKeys(datasetId);
        }

        public override string GetKey(List<string> fieldNames, List<string> fields)
        {
            throw new NotImplementedException();
        }

        public override void Parse(List<string> fieldNames, List<string> fields, int lineNumber, DataRow dr, int edgarDatasetId)
        {
            throw new NotImplementedException();
        }

        public override void BulkCopy(SQLAnalystRepository repo, DataTable dt)
        {
            throw new NotImplementedException();
        }

        public override DataTable GetEmptyDataTable(SQLAnalystRepository repo)
        {
            throw new NotImplementedException();
        }

        public override List<int> GetMissingLinesByTable(IAnalystRepository repo, int datasetId, int totalLines)
        {
            return repo.GetMissingLines(datasetId,"EdgarDatasetDimensions", totalLines);
        }
    }
}