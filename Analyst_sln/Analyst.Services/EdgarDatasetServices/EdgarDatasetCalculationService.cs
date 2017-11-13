using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyst.DBAccess.Contexts;
using System.Collections.Concurrent;

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

        public override void Add(IAnalystRepository repo, EdgarDataset dataset, EdgarDatasetCalculation file)
        {
            repo.Add(dataset,file);
        }

        public override EdgarDatasetCalculation Parse(IAnalystRepository repository, List<string> fieldNames, List<string> fields, int lineNumber)
        {
            EdgarDatasetCalculation calc = new EdgarDatasetCalculation();
            string adsh = fields[fieldNames.IndexOf("adsh")];
            calc.Submission = Submissions[adsh];

            calc.SequentialNumberForGrouping = Convert.ToInt16(fields[fieldNames.IndexOf("grp")]);
            calc.SequentialNumberForArc = Convert.ToInt16(fields[fieldNames.IndexOf("arc")]);

            string pTag = fields[fieldNames.IndexOf("tag")];
            string pVersion = fields[fieldNames.IndexOf("version")];
            calc.ParentTag = Tags[pTag + pVersion];

            string cTag = fields[fieldNames.IndexOf("tag")];
            string cVersion = fields[fieldNames.IndexOf("version")];
            calc.ChildTag = Tags[cTag + cVersion];
            return calc;
        }
    }
}
