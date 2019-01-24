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
using Analyst.Domain.Edgar.Exceptions;
using System.Data;
using Analyst.Services.EdgarServices.EdgarDatasetServices.Interfaces;
using Analyst.DBAccess.Repositories;

namespace Analyst.Services.EdgarDatasetServices.BulkProcessStrategy
{

    public class EdgarDatasetPresentationService : EdgarDatasetBaseService<EdgarDatasetPresentation>, IEdgarDatasetPresentationService
    {
        public ConcurrentDictionary<string, int> Renders { get; set; }
        public ConcurrentDictionary<string, int> Subs { get; set; }
        public ConcurrentDictionary<string, int> Tags { get; set; }

        public ConcurrentDictionary<string, int> Nums { get; set; }
        public ConcurrentDictionary<string, int> Texts { get; set; }

        protected override DatasetsTables RelatedTable { get { return DatasetsTables.Presentations; } }

        private readonly ILog log;
        protected override ILog Log
        {
            get
            {
                return log;
            }
        }
        public EdgarDatasetPresentationService()
        {
            log = log4net.LogManager.GetLogger(this.GetType().Name);
        }

        public override IList<EdgarTuple> GetKeys(IAnalystEdgarDatasetsRepository repository, int datasetId)
        {
            return repository.GetPresentationsKeys(datasetId);
        }

        public override void Parse(List<string> fieldNames, List<string> fields, int lineNumber, DataRow dr, int edgarDatasetId)
        {
            /*
            adsh	                report	line	stmt	inpth	rfile	tag	version	prole	plabel	negating
            0001163302-16-000148	1	    4	    CP	    0	    H	    DocumentFiscalYearFocus	dei/2014	terseLabel	Document Fiscal Year Focus	0
            0001163302-16-000148	1	    3	    CP	    0	    H	    DocumentPeriodEndDate	dei/2014	terseLabel	Document Period End Date	0
            0001125345-17-000041	6	    1	    CF	    0	    H	    NetCashProvidedByUsedInOperatingActivitiesContinuingOperationsAbstract	us-gaap/2015	terseLabel	Operating activities	0
            0001104659-17-016575	111	    6		        0	    H	    NetCashProvidedByUsedInOperatingActivitiesContinuingOperationsAbstract	us-gaap/2015	terseLabel	Cash flows from operating activities:	0
            ...
            */
            try
            {

                string adsh = fields[fieldNames.IndexOf("adsh")];
                string line = fields[fieldNames.IndexOf("line")];
                string report = fields[fieldNames.IndexOf("report")];
                dr["SubmissionId"] = Subs[adsh];
                dr["ReportNumber"] = Convert.ToInt32(report);
                dr["RenderId"] = Renders[adsh + report];
                dr["Line"] = Convert.ToInt32(line);
                dr["FinancialStatement"] = fields[fieldNames.IndexOf("stmt")];
                dr["Inpth"] = fields[fieldNames.IndexOf("inpth")] == "1";
                int index = fieldNames.IndexOf("rfile");
                if (index >= 0)
                    dr["RenderFileStr"] = fields[index][0];
                else
                    dr["RenderFileStr"] = DBNull.Value;
                string tag = fields[fieldNames.IndexOf("tag")];
                string version = fields[fieldNames.IndexOf("version")];
                if (Tags.ContainsKey(tag + version))
                dr["TagId"] = Tags[tag + version];
                else
                {
                    string tag2 = Encoding.GetEncoding(1252).GetString(Encoding.GetEncoding("iso-8859-7").GetBytes(tag));
                    if (Tags.ContainsKey(tag + version))
                        dr["TagId"] = Tags[tag + version];
                    else
                        throw new EdgarLineException(EdgarDatasetPresentation.FILE_NAME, lineNumber, "Error retrieving key: " + tag + version);
                }
                dr["PreferredLabelXBRLLinkRole"] = fields[fieldNames.IndexOf("prole")];
                dr["PreferredLabel"] = fields[fieldNames.IndexOf("plabel")];
                dr["Negating"] = !(fields[fieldNames.IndexOf("negating")] == "0");
                dr["DatasetId"] = edgarDatasetId;
                dr["LineNumber"] = lineNumber;


                string numKey = adsh + tag + version;
                if (Nums.ContainsKey(numKey))
                    dr["NumberId"] = Nums[numKey];
                else
                    dr["ADSH_Tag_Version"] = adsh + " | " + tag + "|" + version;

                if (Texts.ContainsKey(numKey))
                    dr["TextId"] = Texts[numKey];
                else
                    dr["ADSH_Tag_Version"] = adsh + " | " + tag + "|" + version;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
