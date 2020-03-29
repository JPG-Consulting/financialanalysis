using FinancialAnalyst.Common.Entities.EdgarSEC.Indexes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.EdgarSEC.Repositories
{
    public interface IEdgarFilesRepository : IEdgarRepository, IDisposable
    {
        void Add(MasterIndex index);
        IList<MasterIndex> GetFullIndexes();
        MasterIndex GetFullIndex(ushort year, Quarter q);
        void Update(MasterIndex index, string property);
        long GetIndexEntriesCount(MasterIndex index);
    }
}
