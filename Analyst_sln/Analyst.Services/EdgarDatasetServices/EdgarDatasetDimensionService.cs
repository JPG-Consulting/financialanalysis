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

namespace Analyst.Services.EdgarDatasetServices
{
    public interface IEdgarDatasetDimensionService : IEdgarFileService<EdgarDatasetDimension>
    {
        
    }
    public class EdgarDatasetDimensionService : EdgarFileService<EdgarDatasetDimension>, IEdgarDatasetDimensionService
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
            if (file.Id == 0)
                repo.Add(dataset, file);
        }

        public override EdgarDatasetDimension Parse(IAnalystRepository repository, List<string> fieldNames, List<string> fields, int lineNumber, ConcurrentDictionary<string, int> existing)
        {
            string dimhash = fields[fieldNames.IndexOf("dimhash")];
            EdgarDatasetDimension dim;
            if (existing.ContainsKey(dimhash))
                dim = repository.GetDimension(dimhash);
            else
            {
                dim = new EdgarDatasetDimension();
                dim.DimensionH = dimhash;
                dim.Segments = fields[fieldNames.IndexOf("segments")];
                dim.SegmentTruncated = !(fields[fieldNames.IndexOf("segt")] == "0");
                dim.LineNumber = lineNumber;
            }
            return dim;
        }

        public override IList<EdgarTuple> GetKeys(IAnalystRepository repository, int datasetId)
        {
            return repository.GetDimensionKeys(datasetId);
        }
    }
}