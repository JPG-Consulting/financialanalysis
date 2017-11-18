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
    public interface IEdgarDatasetRenderingService : IEdgarFileService<EdgarDatasetRendering>
    {
        ConcurrentDictionary<string, EdgarDatasetSubmission> Subs { get; set; }
    }
    public class EdgarDatasetRenderingService : EdgarFileService<EdgarDatasetRendering>, IEdgarDatasetRenderingService
    {
        public ConcurrentDictionary<string, EdgarDatasetSubmission> Subs { get; set; }

        private readonly ILog log;
        protected override ILog Log
        {
            get
            {
                return log;
            }
        }
        public EdgarDatasetRenderingService()
        {
            log = log4net.LogManager.GetLogger(this.GetType().Name);
        }
        public override void Add(IAnalystRepository repo, EdgarDataset dataset, EdgarDatasetRendering file)
        {
            repo.Add(dataset,file);
        }

        public override EdgarDatasetRendering Parse(IAnalystRepository repository, List<string> fieldNames, List<string> fields, int lineNumber)
        {
            /*
            adsh	report	rfile	menucat	shortname	longname	roleuri	parentroleuri	parentreport	ultparentrpt
            0001163302-16-000148	1	H	Cover	Document and Entity Information	0001000 - Document - Document and Entity Information	http://www.ussteel.com/role/DocumentAndEntityInformation			
            0001163302-16-000148	2	H	Statements	Consolidated Statement Of Operations	1001000 - Statement - Consolidated Statement Of Operations	http://www.ussteel.com/role/ConsolidatedStatementOfOperations			
            */
            EdgarDatasetRendering ren = new EdgarDatasetRendering();
            string adsh = fields[fieldNames.IndexOf("adsh")];
            ren.Submission = Subs[adsh];
            ren.Report = Convert.ToInt32(fields[fieldNames.IndexOf("report")]);
            string value = "";
            ren.RenderFile = fields[fieldNames.IndexOf("rfile")][0];
            ren.MenuCategory = fields[fieldNames.IndexOf("menucat")];
            ren.ShortName = fields[fieldNames.IndexOf("shortname")];
            ren.LongName = fields[fieldNames.IndexOf("longname")];
            ren.Roleuri = fields[fieldNames.IndexOf("roleuri")];
            ren.ParentRoleuri = fields[fieldNames.IndexOf("parentroleuri")];
            value = fields[fieldNames.IndexOf("parentreport")];
            if (!string.IsNullOrEmpty(value))
                ren.ParentReport = Convert.ToInt32(value);
            value = fields[fieldNames.IndexOf("ultparentrpt")];
            if (!string.IsNullOrEmpty(value))
                ren.UltimateParentReport = Convert.ToInt32(value);
            ren.LineNumber = lineNumber;
            return ren;
        }
    }
}
