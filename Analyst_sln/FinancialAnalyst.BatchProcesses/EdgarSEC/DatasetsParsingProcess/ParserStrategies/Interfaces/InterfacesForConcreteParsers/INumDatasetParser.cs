using FinancialAnalyst.Common.Entities.EdgarSEC.Datasets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess.ParserStrategies.Interfaces.InterfacesForConcreteParsers
{
    public interface INumDatasetParser : IEdgarDatasetParser<EdgarDatasetNumber>
    {
        ConcurrentDictionary<string, int> Dimensions { get; set; }
        ConcurrentDictionary<string, int> Submissions { get; set; }
        ConcurrentDictionary<string, int> Tags { get; set; }
    }
}
