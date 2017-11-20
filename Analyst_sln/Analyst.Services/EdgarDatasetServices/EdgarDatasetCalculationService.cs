using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyst.DBAccess.Contexts;
using System.Collections.Concurrent;
using log4net;

namespace Analyst.Services.EdgarDatasetServices
{
    public interface IEdgarDatasetCalculationService:IEdgarFileService<EdgarDatasetCalculation>
    {
        ConcurrentDictionary<string, EdgarDatasetSubmission> Submissions { get; set; }
        ConcurrentDictionary<string, EdgarDatasetTag> Tags { get; set; }
    }
    public class EdgarDatasetCalculationService : EdgarFileService<EdgarDatasetCalculation>, IEdgarDatasetCalculationService
    {

        public ConcurrentDictionary<string, EdgarDatasetSubmission> Submissions { get; set; }
        public ConcurrentDictionary<string, EdgarDatasetTag> Tags { get; set; }

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

        public override EdgarDatasetCalculation Parse(IAnalystRepository repository, List<string> fieldNames, List<string> fields, int lineNumber, ConcurrentDictionary<string, EdgarDatasetCalculation> existing)
        {
            try
            {
                EdgarDatasetCalculation calc = new EdgarDatasetCalculation();
                string adsh = fields[fieldNames.IndexOf("adsh")];
                calc.Submission = Submissions[adsh];

                calc.SequentialNumberForGrouping = Convert.ToInt16(fields[fieldNames.IndexOf("grp")]);
                calc.SequentialNumberForArc = Convert.ToInt16(fields[fieldNames.IndexOf("arc")]);

                string pTag = fields[fieldNames.IndexOf("ptag")];
                string pVersion = fields[fieldNames.IndexOf("pversion")];
                calc.ParentTag = Tags[pTag + pVersion];

                string cTag = fields[fieldNames.IndexOf("ctag")];
                string cVersion = fields[fieldNames.IndexOf("cversion")];
                calc.ChildTag = Tags[cTag + cVersion];

                calc.LineNumber= lineNumber;
                return calc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
