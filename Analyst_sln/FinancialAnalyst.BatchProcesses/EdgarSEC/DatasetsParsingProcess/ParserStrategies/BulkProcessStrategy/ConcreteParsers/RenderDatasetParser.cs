using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Data;
using System.Globalization;
using log4net;
using FinancialAnalyst.Common.Entities.EdgarSEC;
using FinancialAnalyst.Common.Entities.EdgarSEC.Datasets;
using FinancialAnalyst.Common.Entities.EdgarSEC.Repositories;
using FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess.ParserStrategies.BulkProcessStrategy;
using FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess.ParserStrategies.Interfaces.InterfacesForConcreteParsers;

namespace FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess.ParserStrategies.BulkProcessStrategy.ConcreteParsers
{

    public class RenderDatasetParser : BulkEdgarDatasetParser<EdgarDatasetRender>, IRenderDatasetParser
    {
        public ConcurrentDictionary<string, int> Subs { get; set; }

        protected override DatasetsTables RelatedTable { get { return DatasetsTables.Renders; } }

        private readonly ILog log;
        protected override ILog Log
        {
            get
            {
                return log;
            }
        }
        public RenderDatasetParser()
        {
            log = log4net.LogManager.GetLogger(this.GetType());
        }

        public override IList<EdgarTuple> GetKeys(IEdgarDatasetsRepository repository, int datasetId)
        {
            return repository.GetRendersKeys(datasetId);
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

    }
}
