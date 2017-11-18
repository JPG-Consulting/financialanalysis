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

        public override EdgarDatasetDimension Parse(IAnalystRepository repository, List<string> fieldNames, List<string> fields, int lineNumber)
        {
            string dimhash = fields[fieldNames.IndexOf("dimhash")];
            EdgarDatasetDimension dim = repository.GetDimension(dimhash);
            if(dim == null)
            {
                dim = new EdgarDatasetDimension();
                dim.DimensionH = dimhash;
                dim.Segments = fields[fieldNames.IndexOf("segments")];
                dim.SegmentTruncated = !(fields[fieldNames.IndexOf("segt")] == "0");
            }
            return dim;
        }

        
    }
}