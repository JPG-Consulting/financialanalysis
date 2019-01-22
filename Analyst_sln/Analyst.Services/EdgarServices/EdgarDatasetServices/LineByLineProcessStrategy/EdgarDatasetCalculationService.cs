using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyst.DBAccess.Contexts;
using System.Collections.Concurrent;
using log4net;
using Analyst.Domain.Edgar;
using System.Data;
using Analyst.Services.EdgarServices.EdgarDatasetServices.Interfaces;
using Analyst.DBAccess.Repositories;

namespace Analyst.Services.EdgarDatasetServices.LineByLineProcessStrategy
{

    public class EdgarDatasetCalculationService : EdgarDatasetBaseService<EdgarDatasetCalculation>, IEdgarDatasetCalculationService
    {

        public ConcurrentDictionary<string, int> Submissions { get; set; }
        public ConcurrentDictionary<string, int> Tags { get; set; }

        private readonly ILog log;
        protected override ILog Log
        {
            get
            {
                return log;
            }
        }
        public EdgarDatasetCalculationService()
        {
            log = log4net.LogManager.GetLogger(this.GetType().Name);
        }

        public override void Add(IAnalystEdgarDatasetsRepository repo, EdgarDataset dataset, EdgarDatasetCalculation file)
        {
            repo.Add(dataset,file);
        }

        public override EdgarDatasetCalculation Parse(IAnalystEdgarDatasetsRepository repository, List<string> fieldNames, List<string> fields, int lineNumber)
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

        public override IList<EdgarTuple> GetKeys(IAnalystEdgarDatasetsRepository repository, int datasetId)
        {
            return repository.GetCalculationKeys(datasetId);
        }

        
        public override List<int> GetMissingLinesByTable(IAnalystEdgarDatasetsRepository repo, int datasetId, int totalLines)
        {
            return repo.GetMissingLines(datasetId, "EdgarDatasetCalculations", totalLines);
        }
    }
}
