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
using FinancialAnalyst.Common.Exceptions.EdgarSEC;

namespace FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess.ParserStrategies.BulkProcessStrategy.ConcreteParsers
{
    public class CalculationDatasetParser : BulkEdgarDatasetParser<EdgarDatasetCalculation>, ICalculationDatasetParser
    {

        public ConcurrentDictionary<string, int> Submissions { get; set; }
        public ConcurrentDictionary<string, int> Tags { get; set; }

        protected override DatasetsTables RelatedTable { get { return DatasetsTables.Calculations; } }

        private readonly ILog log;
        protected override ILog Log
        {
            get
            {
                return log;
            }
        }
        public CalculationDatasetParser()
        {
            log = log4net.LogManager.GetLogger(this.GetType());
        }

        public override IList<EdgarTuple> GetKeys(IEdgarDatasetsRepository repository, int datasetId)
        {
            return repository.GetCalculationKeys(datasetId);
        }

        public override void Parse(List<string> fieldNames, List<string> fields, int lineNumber, DataRow dr, int edgarDatasetId)
        {
            try
            {
                string adsh = fields[fieldNames.IndexOf("adsh")];
                dr["SubmissionId"] = Submissions[adsh];

                dr["SequentialNumberForGrouping"] = Convert.ToInt16(fields[fieldNames.IndexOf("grp")]);
                dr["SequentialNumberForArc"] = Convert.ToInt16(fields[fieldNames.IndexOf("arc")]);

                //Indicates a weight of -1 (TRUE if the arc is negative), but typically +1 (FALSE).
                dr["Negative"] = fields[fieldNames.IndexOf("negative")] == "-1" ? true : false;

                string pTag = fields[fieldNames.IndexOf("ptag")];
                string pVersion = fields[fieldNames.IndexOf("pversion")];
                if (Tags.ContainsKey(pTag + pVersion))
                    dr["ParentTagId"] = Tags[pTag + pVersion];
                else
                    throw new InvalidLineException($"Key {pTag}|{pVersion} is not present in the Tags dictionary, line number: {lineNumber}");

                string cTag = fields[fieldNames.IndexOf("ctag")];
                string cVersion = fields[fieldNames.IndexOf("cversion")];
                dr["ChildTagId"] = Tags[cTag + cVersion];

                dr["DatasetId"] = edgarDatasetId;

                dr["LineNumber"] = lineNumber;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
