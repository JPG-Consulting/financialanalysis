using Analyst.DBAccess.Repositories;
using Analyst.Domain.Edgar;
using Analyst.Domain.Edgar.Datasets;
using Analyst.Services.EdgarDatasetServices;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Services.EdgarServices.EdgarDatasetServices.Interfaces
{
    public interface IEdgarDatasetBaseService<T> : IEdgarDatasetTemplateProcessBaseService<T> where T : class, IEdgarDatasetFile
    {
        void ProcessFile(ConcurrentBag<int> missing, string fileToProcess, string fieldToUpdate, EdgarTaskState state, string[] allLines, string header, string cacheFolder, string tsvFileName, bool processInParallel);

        IList<EdgarTuple> GetKeys(IAnalystEdgarDatasetsRepository repository, int datasetId);
    }
}
