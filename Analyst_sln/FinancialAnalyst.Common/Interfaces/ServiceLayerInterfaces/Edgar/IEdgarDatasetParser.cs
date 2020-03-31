using FinancialAnalyst.Common.Entities.EdgarSEC.Datasets;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.Edgar
{
    public interface IEdgarDatasetParser : IDisposable
    {
        IList<EdgarDataset> GetDatasets();
        EdgarDataset GetDataset(int id);
        bool IsRunning(int id);
        void ProcessDataset(int id);
        void DeleteDatasetFile(int id, string file);
    }
}
