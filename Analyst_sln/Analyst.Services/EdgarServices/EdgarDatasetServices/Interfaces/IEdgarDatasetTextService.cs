using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Services.EdgarServices.EdgarDatasetServices.Interfaces
{
    public interface IEdgarDatasetTextService : IEdgarDatasetBaseService<EdgarDatasetText>
    {
        ConcurrentDictionary<string, int> Submissions { get; set; }
        ConcurrentDictionary<string, int> Tags { get; set; }
        ConcurrentDictionary<string, int> Dimensions { get; set; }

    }
}
