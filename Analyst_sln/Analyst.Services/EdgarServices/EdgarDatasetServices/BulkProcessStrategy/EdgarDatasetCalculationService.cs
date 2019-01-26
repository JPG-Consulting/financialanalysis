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
using Analyst.Services.EdgarServices.EdgarDatasetServices;

namespace Analyst.Services.EdgarDatasetServices.BulkProcessStrategy
{
    public class EdgarDatasetCalculationService : EdgarDatasetBaseService<EdgarDatasetCalculation>, IEdgarDatasetCalculationService
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
        public EdgarDatasetCalculationService()
        {
            log = log4net.LogManager.GetLogger(this.GetType().Name);
        }

        public override IList<EdgarTuple> GetKeys(IAnalystEdgarDatasetsRepository repository, int datasetId)
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
