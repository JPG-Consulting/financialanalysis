using FinancialAnalyst.Common.Entities.EdgarSEC.Datasets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess.ParserStrategies.Interfaces.InterfacesForConcreteParsers
{
    public interface IPresentationDatasetParser : IEdgarDatasetParser<EdgarDatasetPresentation>
    {
        ConcurrentDictionary<string, int> Nums { get; set; }
        ConcurrentDictionary<string, int> Renders { get; set; }
        ConcurrentDictionary<string, int> Subs { get; set; }
        ConcurrentDictionary<string, int> Tags { get; set; }
        ConcurrentDictionary<string, int> Texts { get; set; }
    }
}
