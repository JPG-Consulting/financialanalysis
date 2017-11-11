using System;
using System.Collections.Concurrent;
using Analyst.Domain.Edgar.Datasets;
using Analyst.DBAccess.Contexts;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Analyst.Services.EdgarDatasetServices
{
    public interface IEdgarDatasetDimensionService : IEdgarFileService<EdgarDatasetDimension>
    {
        
    }
    public class EdgarDatasetDimensionService : EdgarFileService<EdgarDatasetDimension>, IEdgarDatasetDimensionService
    {

        public override void Add(IAnalystRepository repo, EdgarDataset dataset, EdgarDatasetDimension file)
        {
            if (file.Id == 0)
                repo.AddDimension(dataset, file);
        }

        public override EdgarDatasetDimension Parse(IAnalystRepository repository, string header, string line, int linenumber)
        {
            List<string> fieldNames = header.Split('\t').ToList();
            List<string> fields = line.Split('\t').ToList();
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