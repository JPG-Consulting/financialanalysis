using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Data;
using System.Globalization;
using log4net;
using FinancialAnalyst.Common.Entities.EdgarSEC;
using FinancialAnalyst.Common.Entities.EdgarSEC.Datasets;
using FinancialAnalyst.Common.Entities.EdgarSEC.Repositories;
using FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess.ParserStrategies.BulkProcessStrategy;
using FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess.ParserStrategies.Interfaces.InterfacesForConcreteParsers;

namespace FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess.ParserStrategies.BulkProcessStrategy.ConcreteParsers
{

    public class DimensionDatasetParser : BulkEdgarDatasetParser<EdgarDatasetDimension>, IDimensionDatasetParser
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

        public override IList<EdgarTuple> GetKeys(IEdgarDatasetsRepository repository, int datasetId)
        {
            return repository.GetDimensionKeys(datasetId);
        }
        
        public override void Parse(List<string> fieldNames, List<string> fields, int lineNumber, DataRow dr, int edgarDatasetId)
        {
            dr["DimensionH"] = fields[fieldNames.IndexOf("dimhash")]; ;
            dr["Segments"] = fields[fieldNames.IndexOf("segments")];
            dr["SegmentTruncated"] = !(fields[fieldNames.IndexOf("segt")] == "0");
            dr["LineNumber"] = lineNumber;
            dr["DatasetId"] = edgarDatasetId;
        }
    }
}