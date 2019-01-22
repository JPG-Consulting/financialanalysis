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
        public override void Add(IAnalystEdgarDatasetsRepository repo, EdgarDataset dataset, EdgarDatasetRender file)
        {
            repo.Add(dataset,file);
        }

        public override EdgarDatasetRender Parse(IAnalystEdgarDatasetsRepository repository, List<string> fieldNames, List<string> fields, int lineNumber)
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
            ren.RenderFileStr = ren.RenderFile.ToString(); 
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

        public override IList<EdgarTuple> GetKeys(IAnalystEdgarDatasetsRepository repository, int datasetId)
        {
            return repository.GetRendersKeys(datasetId);
        }

        
        public override List<int> GetMissingLinesByTable(IAnalystEdgarDatasetsRepository repo, int datasetId, int totalLines)
        {
            return repo.GetMissingLines(datasetId,"EdgarDatasetRenders", totalLines);
        }
    }
}
