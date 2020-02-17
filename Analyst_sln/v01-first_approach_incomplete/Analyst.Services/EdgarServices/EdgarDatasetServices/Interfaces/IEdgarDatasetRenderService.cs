using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Services.EdgarServices.EdgarDatasetServices.Interfaces
{
    public interface IEdgarDatasetRenderService : IEdgarDatasetBaseService<EdgarDatasetRender>
    {
        ConcurrentDictionary<string, int> Subs { get; set; }
    }
}
