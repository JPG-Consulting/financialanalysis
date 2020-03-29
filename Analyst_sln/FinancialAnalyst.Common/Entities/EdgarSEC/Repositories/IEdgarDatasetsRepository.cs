using FinancialAnalyst.Common.Entities.EdgarSEC.Datasets;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.EdgarSEC.Repositories
{
    public interface IEdgarDatasetsRepository : IEdgarRepository, IDisposable
    {
        int GetCount<T>(int datasetId) where T : class, IEdgarDatasetFile;
        int GetDatasetsCount();

        EdgarDataset GetDataset(int id);

        IList<EdgarTuple> GetCalculationKeys(int datasetId);
        IList<EdgarTuple> GetTagsKeys(int datasetId);
        IList<EdgarTuple> GetTextKeys(int datasetId);
        IList<EdgarTuple> GetDimensionKeys(int datasetId);
        IList<EdgarTuple> GetNumberKeys(int datasetId);
        IList<EdgarTuple> GetSubmissionKeys(int datasetId);
        IList<EdgarTuple> GetRendersKeys(int datasetId);
        IList<EdgarTuple> GetPresentationsKeys(int datasetId);

        void Add(Registrant r);
        void Add(EdgarDataset dataset);
        void Add(EdgarDataset dataset, EdgarDatasetSubmission sub);
        void Add(EdgarDataset dataset, EdgarDatasetTag tag);
        void Add(EdgarDataset dataset, EdgarDatasetNumber number);
        void Add(EdgarDataset dataset, EdgarDatasetDimension dim);
        void Add(EdgarDataset dataset, EdgarDatasetRender ren);
        void Add(EdgarDataset dataset, EdgarDatasetPresentation pre);
        void Add(EdgarDataset dataset, EdgarDatasetCalculation file);
        void Add(EdgarDataset dataset, EdgarDatasetText file);

        void UpdateEdgarDataset(EdgarDataset dataset, string property);
        List<int> GetMissingLines(int datasetId, DatasetsTables table, int totalLines);
        void EnablePresentationIndexes(bool enable);
    }
}
