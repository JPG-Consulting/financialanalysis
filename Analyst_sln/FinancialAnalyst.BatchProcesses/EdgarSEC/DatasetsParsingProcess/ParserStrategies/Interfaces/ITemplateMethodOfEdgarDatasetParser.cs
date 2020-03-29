using FinancialAnalyst.Common.Entities.EdgarSEC.Datasets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess.ParserStrategies.Interfaces
{
    public interface ITemplateMethodOfEdgarDatasetParser<T> : IDisposable where T : class, IEdgarDatasetFile
    {
        ConcurrentDictionary<string, int> GetAsConcurrent(int datasetId);
        void Process(EdgarTaskState state, bool processInParallel, string fileToProcess, string fieldToUpdate);
    }
}
