using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyst.DBAccess.Contexts;
using System.Collections.Concurrent;
using System.Globalization;
using log4net;
using Analyst.Domain.Edgar;
using System.Data;

namespace Analyst.Services.EdgarDatasetServices
{
    public interface IEdgarDatasetTextService: IEdgarDatasetBaseService<EdgarDatasetText>
    {
        ConcurrentDictionary<string, int> Submissions { get; set; }
        ConcurrentDictionary<string, int> Tags { get; set; }

        ConcurrentDictionary<string, int> Dimensions { get; set; }

    }
    public class EdgarDatasetTextService : EdgarDatasetBaseService<EdgarDatasetText>, IEdgarDatasetTextService
    {
        public ConcurrentDictionary<string, int> Submissions { get; set; }
        public ConcurrentDictionary<string, int> Tags { get; set; }

        public ConcurrentDictionary<string, int> Dimensions { get; set; }

        private readonly ILog log;
        protected override ILog Log
        {
            get
            {
                return log;
            }
        }
        public EdgarDatasetTextService()
        {
            log = log4net.LogManager.GetLogger(this.GetType().Name);
        }
        public override void Add(IAnalystRepository repo, EdgarDataset dataset, EdgarDatasetText file)
        {
            repo.Add(dataset, file);
        }

        public override EdgarDatasetText Parse(IAnalystRepository repository, List<string> fieldNames, List<string> fields, int lineNumber)
        {
            /*
            adsh	tag	version	ddate	qtrs	iprx	lang	dcml	durp	datp	dimh	dimn	coreg	escaped	srclen	txtlen	footnote	footlen	context	value
            0001163302-16-000148	AssetRetirementObligationDisclosureTextBlock	us-gaap/2016	20160930	3	0	en-US	32767	0.008219957	0.0	0x00000000	0		1	17336	907		0	FD2016Q3YTD	Asset Retirement Obligations U. S. Steels asset retirement obligations (AROs) primarily relate to mine and landfill closure and post-closure costs. The following table reflects changes in the carrying values of AROs: (In millions) September�30, 2016 December�31, 2015 Balance at beginning of year $ 89 $ 48 Additional obligations incurred 3 45 (a) Obligations settled (9 ) (6 ) Foreign currency translation effects  (1 ) Accretion expense 2 3 Balance at end of period $ 85 $ 89 (a) Additional AROs relate to the permanent closure of the coke production facilities at Gary Works and Granite City Works. Certain AROs related to disposal costs of the majority of fixed assets at our integrated steel facilities have not been recorded because they have an indeterminate settlement date. These AROs will be initially recognized in the period in which sufficient information exists to estimate their fair value.
            0001163302-16-000148	BasisOfPresentationAndSignificantAccountingPoliciesTextBlock	us-gaap/2016	20160930	3	0	en-US	32767	0.008219957	0.0	0x00000000	0		1	3331	2042		0	FD2016Q3YTD	Basis of Presentation and Significant Accounting Policies United States Steel Corporation produces and sells steel products, including flat-rolled and tubular products, in North America and Central Europe. Operations in North America also include iron ore and coke production facilities, railroad services and real estate operations. Operations in Europe also include coke production facilities. The year-end Consolidated Balance Sheet data was derived from audited statements but does not include all disclosures required for complete financial statements by accounting principles generally accepted in the United States of America (U.S. GAAP). The other information in these financial statements is unaudited but, in the opinion of management, reflects all adjustments necessary for a fair statement of the results for the periods covered. All such adjustments are of a normal recurring nature unless disclosed otherwise. These financial statements, including notes, have been prepared in accordance with the applicable rules of the Securities and Exchange Commission and do not include all of the information and disclosures required by U.S. GAAP for complete financial statements. Additional information is contained in the United States Steel Corporation Annual Report on Form�10-K for the fiscal year ended December�31, 2015 , which should be read in conjunction with these financial statements. Revision of Prior Period Financial Statements During 2015, the Company identified a prior period error related to the classification of unpaid capital expenditures in the Consolidated Statements of Cash Flows that impacted the quarterly interim financial statements in 2015. As a result, the Consolidated Statement of Cash Flows for the the nine months ended September 30, 2015 has been revised to reflect a decrease in cash provided by operating activities and a decrease in cash used in investing activities of $55 million . The Company has concluded the impact of this error was not material to the previously filed financial statements.
            */

            //aca puede iterar sobre la coleccion como en el proyecto sec q db

            EdgarDatasetText text = new EdgarDatasetText();
            string adsh = fields[fieldNames.IndexOf("adsh")];
            text.SubmissionId = Submissions[adsh];
            string tag = fields[fieldNames.IndexOf("tag")];
            string version = fields[fieldNames.IndexOf("version")];
            text.TagId = Tags[tag+version];
            string value = fields[fieldNames.IndexOf("ddate")];
            text.DatavalueEnddate = new DateTime(int.Parse(value.Substring(0, 4)), int.Parse(value.Substring(4, 2)), int.Parse(value.Substring(6, 2)));
            text.CountOfNumberOfQuarters = Convert.ToInt32(fields[fieldNames.IndexOf("qtrs")]);
            text.Iprx = Convert.ToInt16(fields[fieldNames.IndexOf("iprx")]);
            text.Language = fields[fieldNames.IndexOf("lang")];
            text.Dcml = Convert.ToInt32(fields[fieldNames.IndexOf("dcml")]);
            text.Durp = float.Parse(fields[fieldNames.IndexOf("durp")], CultureInfo.GetCultureInfo("en-us").NumberFormat);
            text.Datp = float.Parse(fields[fieldNames.IndexOf("datp")], CultureInfo.GetCultureInfo("en-us").NumberFormat);
            text.DimensionId = Dimensions[fields[fieldNames.IndexOf("dimh")]];
            value = fields[fieldNames.IndexOf("dimn")];
            if (!string.IsNullOrEmpty(value))
                text.DimensionNumber = Convert.ToInt16(value);
            value = fields[fieldNames.IndexOf("coreg")];
            if (string.IsNullOrEmpty(value))
                text.CoRegistrant = null;
            else
                text.CoRegistrant = value;
            text.Escaped = fields[fieldNames.IndexOf("escaped")]!= "0";
            text.SourceLength = Convert.ToInt32(fields[fieldNames.IndexOf("srclen")]);
            text.TextLength = Convert.ToInt32(fields[fieldNames.IndexOf("txtlen")]);
            text.FootNote = fields[fieldNames.IndexOf("footnote")];
            value = fields[fieldNames.IndexOf("footlen")];
            if (!string.IsNullOrEmpty(value))
                text.FootLength = Convert.ToInt32(value); ;
            text.Context = fields[fieldNames.IndexOf("context")];
            text.Value = fields[fieldNames.IndexOf("value")];
            text.LineNumber = lineNumber;
            return text;
        }


        public override IList<EdgarTuple> GetKeys(IAnalystRepository repository, int datasetId)
        {
            return repository.GetTextKeys(datasetId);
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
            return repo.GetMissingLines(datasetId,"EdgarDatasetTexts", totalLines);
        }
    }

}
