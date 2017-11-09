using Analyst.DBAccess.Contexts;
using Analyst.Domain.Edgar.Datasets;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Analyst.Services.EdgarServices
{
    public interface IEdgarFileService<T> where T : IEdgarDatasetFile
    {
        ConcurrentDictionary<string, T> GetAsConcurrent();
    }

    public abstract class EdgarFileService<T>:IEdgarFileService<T> where T:IEdgarDatasetFile
    {

        public ConcurrentDictionary<string, T> GetAsConcurrent()
        {
            ConcurrentDictionary<string, T> ret = new ConcurrentDictionary<string, T>();
            IAnalystRepository repository = new AnalystRepository(new AnalystContext());
            IList<T> xs = repository.Get<T>();
            foreach (T x in xs)
            {
                ret.TryAdd(x.Key, x);
            }
            return ret;
        }
    }
}