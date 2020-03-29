using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using log4net;
using FinancialAnalyst.Common.Entities.EdgarSEC;
using FinancialAnalyst.Common.Entities.EdgarSEC.Datasets;
using FinancialAnalyst.Common.Entities.EdgarSEC.Repositories;
using FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess.ParserStrategies.Interfaces.InterfacesForConcreteParsers;

namespace FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess.ParserStrategies.LineByLineProcessStrategy.ConcreteParsers
{
    public class DimensionDatasetParser : LineByLineEdgarDatasetParser<EdgarDatasetDimension>, IDimensionDatasetParser
    {
        protected override DatasetsTables RelatedTable { get { return DatasetsTables.Dimensions; } }

        private readonly ILog log;
        protected override ILog Log
        {
            get
            {
                return log;
            }
        }
        public DimensionDatasetParser()
        {
            log = log4net.LogManager.GetLogger(this.GetType());
        }
        public override void Add(IEdgarDatasetsRepository repo, EdgarDataset dataset, EdgarDatasetDimension file)
        {
            repo.Add(dataset, file);
        }

        public override EdgarDatasetDimension Parse(IEdgarDatasetsRepository repository, List<string> fieldNames, List<string> fields, int lineNumber)
        {
            string dimhash = fields[fieldNames.IndexOf("dimhash")];
            EdgarDatasetDimension dim;
            dim = new EdgarDatasetDimension();
            dim.DimensionH = dimhash;
            dim.Segments = fields[fieldNames.IndexOf("segments")];
            dim.SegmentTruncated = !(fields[fieldNames.IndexOf("segt")] == "0");
            dim.LineNumber = lineNumber;
            return dim;
        }

        public override IList<EdgarTuple> GetKeys(IEdgarDatasetsRepository repository, int datasetId)
        {
            return repository.GetDimensionKeys(datasetId);
        }

    }
}