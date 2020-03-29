using FinancialAnalyst.Common.Entities.EdgarSEC;
using FinancialAnalyst.Common.Entities.EdgarSEC.Datasets;
using FinancialAnalyst.Common.Entities.EdgarSEC.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess.ParserStrategies.Interfaces
{
    public interface IEdgarDatasetParser<T> : ITemplateMethodOfEdgarDatasetParser<T> where T : class, IEdgarDatasetFile
    {
        void ProcessFile(ConcurrentBag<int> missing, string fileToProcess, string fieldToUpdate, EdgarTaskState state, string[] allLines, string header, string cacheFolder, string tsvFileName, bool processInParallel);

        IList<EdgarTuple> GetKeys(IEdgarDatasetsRepository repository, int datasetId);
    }
}
