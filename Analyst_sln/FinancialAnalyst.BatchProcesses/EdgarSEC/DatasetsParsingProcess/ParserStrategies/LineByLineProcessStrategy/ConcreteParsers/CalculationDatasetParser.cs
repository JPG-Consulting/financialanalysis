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

    public class CalculationDatasetParser : LineByLineEdgarDatasetParser<EdgarDatasetCalculation>, ICalculationDatasetParser
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

        public override void Add(IEdgarDatasetsRepository repo, EdgarDataset dataset, EdgarDatasetCalculation file)
        {
            repo.Add(dataset,file);
        }

        public override EdgarDatasetCalculation Parse(IEdgarDatasetsRepository repository, List<string> fieldNames, List<string> fields, int lineNumber)
        {
            try
            {
                EdgarDatasetCalculation calc = new EdgarDatasetCalculation();

                string adsh = fields[fieldNames.IndexOf("adsh")];
                calc.SubmissionId = Submissions[adsh];

                calc.SequentialNumberForGrouping = Convert.ToInt16(fields[fieldNames.IndexOf("grp")]);
                calc.SequentialNumberForArc = Convert.ToInt16(fields[fieldNames.IndexOf("arc")]);

                string pTag = fields[fieldNames.IndexOf("ptag")];
                string pVersion = fields[fieldNames.IndexOf("pversion")];
                calc.ParentTagId = Tags[pTag + pVersion];

                //Indicates a weight of -1 (TRUE if the arc is negative), but typically +1 (FALSE).
                calc.Negative = fields[fieldNames.IndexOf("negative")] == "-1" ? true : false;

                string cTag = fields[fieldNames.IndexOf("ctag")];
                string cVersion = fields[fieldNames.IndexOf("cversion")];
                calc.ChildTagId = Tags[cTag + cVersion];

                calc.LineNumber= lineNumber;
                return calc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override IList<EdgarTuple> GetKeys(IEdgarDatasetsRepository repository, int datasetId)
        {
            return repository.GetCalculationKeys(datasetId);
        }
    }
}
