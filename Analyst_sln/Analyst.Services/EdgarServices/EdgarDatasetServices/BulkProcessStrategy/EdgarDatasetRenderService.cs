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

namespace Analyst.Services.EdgarDatasetServices.BulkProcessStrategy
{

    public class EdgarDatasetRenderService : EdgarDatasetBaseService<EdgarDatasetRender>, IEdgarDatasetRenderService
    {
        public ConcurrentDictionary<string, int> Subs { get; set; }

        private readonly ILog log;
        protected override ILog Log
        {
            get
            {
                return log;
            }
        }
        public EdgarDatasetRenderService()
        {
            log = log4net.LogManager.GetLogger(this.GetType().Name);
        }

        public override IList<EdgarTuple> GetKeys(IAnalystEdgarDatasetsRepository repository, int datasetId)
        {
            return repository.GetRendersKeys(datasetId);
        }

        public override void BulkCopy(IAnalystEdgarDatasetsBulkRepository repo, DataTable dt)
        {
            repo.BulkCopyRenders(dt);
        }

        public override DataTable GetEmptyDataTable(IAnalystEdgarDatasetsBulkRepository repo)
        {
            return repo.GetEmptyRenderDataTable();
        }

        public override void Parse(List<string> fieldNames, List<string> fields, int lineNumber, DataRow dr, int edgarDatasetId)
        {
            
            string adsh = fields[fieldNames.IndexOf("adsh")];
            dr["SubmissionId"] = Subs[adsh];
            dr["Report"] = Convert.ToInt32(fields[fieldNames.IndexOf("report")]);
            string value = "";
            dr["RenderFileStr"] = fields[fieldNames.IndexOf("rfile")][0];
            dr["MenuCategory"] = fields[fieldNames.IndexOf("menucat")];
            dr["ShortName"] = fields[fieldNames.IndexOf("shortname")];
            dr["LongName"] = fields[fieldNames.IndexOf("longname")];
            dr["RoleURI"] = fields[fieldNames.IndexOf("roleuri")];
            dr["ParentRoleURI"] = fields[fieldNames.IndexOf("parentroleuri")];
            value = fields[fieldNames.IndexOf("parentreport")];
            if (!string.IsNullOrEmpty(value))
                dr["ParentReport"] = Convert.ToInt32(value);
            value = fields[fieldNames.IndexOf("ultparentrpt")];
            if (!string.IsNullOrEmpty(value))
                dr["UltimateParentReport"] = Convert.ToInt32(value);
            dr["DatasetId"] = edgarDatasetId;
            dr["LineNumber"] = lineNumber;

        }

        public override List<int> GetMissingLinesByTable(IAnalystEdgarDatasetsRepository repo, int datasetId, int totalLines)
        {
            return repo.GetMissingLines(datasetId,"EdgarDatasetRenders", totalLines);
        }
    }
}
