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

        public override string GetKey(List<string> fieldNames, List<string> fields)
        {
            throw new NotImplementedException();
        }

        public override void Parse(List<string> fieldNames, List<string> fields, int lineNumber, DataRow dr, int edgarDatasetId)
        {
            dr["DimensionH"] = fields[fieldNames.IndexOf("dimhash")]; ;
            dr["Segments"] = fields[fieldNames.IndexOf("segments")];
            dr["SegmentTruncated"] = !(fields[fieldNames.IndexOf("segt")] == "0");
            dr["LineNumber"] = lineNumber;
            dr["DatasetId"] = edgarDatasetId;
        }

        public override void BulkCopy(SQLAnalystEdgarDatasetsRepository repo, DataTable dt)
        {
            repo.BulkCopyDimensions(dt);
        }

        public override DataTable GetEmptyDataTable(SQLAnalystEdgarDatasetsRepository repo)
        {
            return repo.GetEmptyDimensionsDataTable();
        }

        public override List<int> GetMissingLinesByTable(IAnalystEdgarDatasetsRepository repo, int datasetId, int totalLines)
        {
            return repo.GetMissingLines(datasetId,"EdgarDatasetDimensions", totalLines);
        }
    }
}