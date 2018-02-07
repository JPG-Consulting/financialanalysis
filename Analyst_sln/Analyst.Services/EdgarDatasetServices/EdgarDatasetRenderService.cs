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
    public interface IEdgarDatasetRenderService : IEdgarDatasetBaseService<EdgarDatasetRender>
    {
        ConcurrentDictionary<string, int> Subs { get; set; }
    }
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
        public override void Add(IAnalystRepository repo, EdgarDataset dataset, EdgarDatasetRender file)
        {
            repo.Add(dataset,file);
        }

        public override EdgarDatasetRender Parse(IAnalystRepository repository, List<string> fieldNames, List<string> fields, int lineNumber)
        {
            /*
            adsh	report	rfile	menucat	shortname	longname	roleuri	parentroleuri	parentreport	ultparentrpt
            0001163302-16-000148	1	H	Cover	Document and Entity Information	0001000 - Document - Document and Entity Information	http://www.ussteel.com/role/DocumentAndEntityInformation			
            0001163302-16-000148	2	H	Statements	Consolidated Statement Of Operations	1001000 - Statement - Consolidated Statement Of Operations	http://www.ussteel.com/role/ConsolidatedStatementOfOperations			
            */
            EdgarDatasetRender ren = new EdgarDatasetRender();
            string adsh = fields[fieldNames.IndexOf("adsh")];
            ren.SubmissionId = Subs[adsh];
            ren.Report = Convert.ToInt32(fields[fieldNames.IndexOf("report")]);
            string value = "";
            ren.RenderFile = fields[fieldNames.IndexOf("rfile")][0];
            ren.MenuCategory = fields[fieldNames.IndexOf("menucat")];
            ren.ShortName = fields[fieldNames.IndexOf("shortname")];
            ren.LongName = fields[fieldNames.IndexOf("longname")];
            ren.RoleURI = fields[fieldNames.IndexOf("roleuri")];
            ren.ParentRoleURI = fields[fieldNames.IndexOf("parentroleuri")];
            value = fields[fieldNames.IndexOf("parentreport")];
            if (!string.IsNullOrEmpty(value))
                ren.ParentReport = Convert.ToInt32(value);
            value = fields[fieldNames.IndexOf("ultparentrpt")];
            if (!string.IsNullOrEmpty(value))
                ren.UltimateParentReport = Convert.ToInt32(value);
            ren.LineNumber = lineNumber;
            return ren;
        }

        public override IList<EdgarTuple> GetKeys(IAnalystRepository repository, int datasetId)
        {
            return repository.GetRendersKeys(datasetId);
        }

        public override string GetKey(List<string> fieldNames, List<string> fields)
        {
            throw new NotImplementedException();
        }

        public override void BulkCopy(SQLAnalystRepository repo, DataTable dt)
        {
            repo.BulkCopyRenders(dt);
        }

        public override DataTable GetEmptyDataTable(SQLAnalystRepository repo)
        {
            return repo.GetEmptyRenderDataTable();
        }

        public override void Parse(List<string> fieldNames, List<string> fields, int lineNumber, DataRow dr, int edgarDatasetId)
        {
            
            string adsh = fields[fieldNames.IndexOf("adsh")];
            dr["SubmissionId"] = Subs[adsh];
            dr["Report"] = Convert.ToInt32(fields[fieldNames.IndexOf("report")]);
            string value = "";
            //dr["RenderFile"] = fields[fieldNames.IndexOf("rfile")][0];//char datatypes are not mapped directly, it has to be mapped using string or fluent
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

        public override List<int> GetMissingLinesByTable(IAnalystRepository repo, int datasetId, int totalLines)
        {
            return repo.GetMissingLines(datasetId,"EdgarDatasetRenders", totalLines);
        }
    }
}
