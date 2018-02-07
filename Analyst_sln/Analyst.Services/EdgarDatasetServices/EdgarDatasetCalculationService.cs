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

namespace Analyst.Services.EdgarDatasetServices
{
    public interface IEdgarDatasetCalculationService: IEdgarDatasetBaseService<EdgarDatasetCalculation>
    {
        ConcurrentDictionary<string, int> Submissions { get; set; }
        ConcurrentDictionary<string, int> Tags { get; set; }
    }
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

        public override void Add(IAnalystRepository repo, EdgarDataset dataset, EdgarDatasetCalculation file)
        {
            repo.Add(dataset,file);
        }

        public override EdgarDatasetCalculation Parse(IAnalystRepository repository, List<string> fieldNames, List<string> fields, int lineNumber)
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

        public override IList<EdgarTuple> GetKeys(IAnalystRepository repository, int datasetId)
        {
            return repository.GetCalculationKeys(datasetId);
        }

        public override string GetKey(List<string> fieldNames, List<string> fields)
        {
            throw new NotImplementedException();
        }

        public override void Parse(List<string> fieldNames, List<string> fields, int lineNumber, DataRow dr, int edgarDatasetId)
        {
            throw new NotImplementedException();
        }

        public override void BulkCopy(SQLAnalystRepository repo, DataTable dt)
        {
            throw new NotImplementedException();
        }

        public override DataTable GetEmptyDataTable(SQLAnalystRepository repo)
        {
            throw new NotImplementedException();
        }

        public override List<int> GetMissingLinesByTable(IAnalystRepository repo, int datasetId, int totalLines)
        {
            return repo.GetMissingLines(datasetId, "EdgarDatasetCalculations", totalLines);
        }
    }
}
