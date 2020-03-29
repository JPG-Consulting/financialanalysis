using FinancialAnalyst.Common.Entities.EdgarSEC.Datasets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess.ParserStrategies.Interfaces.InterfacesForConcreteParsers
{
    public interface IRenderDatasetParser : IEdgarDatasetParser<EdgarDatasetRender>
    {
        ConcurrentDictionary<string, int> Subs { get; set; }
    }
}
