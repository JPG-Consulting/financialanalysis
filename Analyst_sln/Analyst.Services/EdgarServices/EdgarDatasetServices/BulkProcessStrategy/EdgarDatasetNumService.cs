using Analyst.DBAccess.Contexts;
using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Globalization;
using log4net;
using Analyst.Domain.Edgar;
using System.Data;
using Analyst.Services.EdgarServices.EdgarDatasetServices.Interfaces;
using Analyst.DBAccess.Repositories;

namespace Analyst.Services.EdgarDatasetServices.BulkProcessStrategy
{
    public class EdgarDatasetNumService: EdgarDatasetBaseService<EdgarDatasetNumber>, IEdgarDatasetNumService
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
        public EdgarDatasetNumService()
        {
            log = log4net.LogManager.GetLogger(this.GetType().Name);
        }

        public override IList<EdgarTuple> GetKeys(IAnalystEdgarDatasetsRepository repository, int datasetId)
        {
            return repository.GetNumberKeys(datasetId);
        }

        public override List<int> GetMissingLinesByTable(IAnalystEdgarDatasetsRepository repo, int datasetId, int totalLines)
        {
            return repo.GetMissingLines(datasetId,"EdgarDatasetNumbers", totalLines);
        }

        public override void Parse(List<string> fieldNames, List<string> fields, int lineNumber, DataRow dr, int edgarDatasetId)
        {
            /*
            Ejemplo
            adsh	tag	version	ddate	qtrs	uom	dimh	iprx	value	footnote	footlen	dimn	coreg	durp	datp	dcml
            0000846913-16-000146	ShareBasedCompensationArrangementByShareBasedPaymentAwardOptionsVestedAndExpectedToVestExercisableAggregateIntrinsicValue	us-gaap/2015	20160930	0	USD	0x00000000	0	0.0000		0	0		0.0	0.0	-3
            0000846913-16-000146	WeightedAverageNumberOfSharesRestrictedStock	us-gaap/2015	20150930	1	shares	0x00000000	0	0.0000		0	0		0.0027400255	0.0	-3
            */
            String value = "";
            
            value = fields[fieldNames.IndexOf("ddate")];
            dr["DatavalueEnddate"] = new DateTime(int.Parse(value.Substring(0, 4)), int.Parse(value.Substring(4, 2)), int.Parse(value.Substring(6, 2)));
            value = fields[fieldNames.IndexOf("qtrs")];
            dr["CountOfNumberOfQuarters"] = Convert.ToInt32(value);
            dr["UnitOfMeasure"] = fields[fieldNames.IndexOf("uom")];
            
            value = fields[fieldNames.IndexOf("iprx")];
            dr["IPRX"] = Convert.ToInt16(value);
            value = fields[fieldNames.IndexOf("value")];
            if (String.IsNullOrEmpty(value))
                dr["Value"] = DBNull.Value;
            else
                dr["Value"] = double.Parse(value, CultureInfo.GetCultureInfo("en-us").NumberFormat);
            value = fields[fieldNames.IndexOf("footnote")];
            if (string.IsNullOrEmpty(value))
                dr["FootNote"] = DBNull.Value;
            else
                dr["FootNote"] = value;
            value = fields[fieldNames.IndexOf("footlen")];
            dr["FootLength"] = Convert.ToInt16(value);
            value = fields[fieldNames.IndexOf("dimn")];
            dr["NumberOfDimensions"] = Convert.ToInt16(value);
            value = fields[fieldNames.IndexOf("coreg")];
            if (string.IsNullOrEmpty(value))
                dr["CoRegistrant"] = DBNull.Value;
            else
                dr["CoRegistrant"] = value;
            value = fields[fieldNames.IndexOf("durp")];
            dr["durp"] = float.Parse(value, CultureInfo.GetCultureInfo("en-us").NumberFormat);
            value = fields[fieldNames.IndexOf("datp")];
            dr["datp"] = float.Parse(value, CultureInfo.GetCultureInfo("en-us").NumberFormat);
            value = fields[fieldNames.IndexOf("dcml")];
            dr["Decimals"] = Convert.ToInt32(value);
            
            dr["LineNumber"] = lineNumber;

            dr["DatasetId"] = edgarDatasetId;

            string adsh  = fields[fieldNames.IndexOf("adsh")];
            string tag = fields[fieldNames.IndexOf("tag")];
            string version = fields[fieldNames.IndexOf("version")];
            string dimh = fields[fieldNames.IndexOf("dimh")];

            //if (!Submissions.ContainsKey(adsh)) throw new KeyNotFoundException("Submissions[" + adsh + "]");
            dr["SubmissionId"] = Submissions[adsh];

            //if (!Tags.ContainsKey(tag + version)) throw new KeyNotFoundException("Tags[" + tag + version + "]");
            dr["TagId"] = Tags[tag + version];

            //if (!Dimensions.ContainsKey(dimh)) throw new KeyNotFoundException("Dimensions[" + dimh + "]");
            dr["DimensionId"] = Dimensions[dimh];

        }

        public override void BulkCopy(IAnalystEdgarDatasetsBulkRepository repo, DataTable dt)
        {
            repo.BulkCopyNumbers(dt);
        }

        public override DataTable GetEmptyDataTable(IAnalystEdgarDatasetsBulkRepository repo)
        {
            return repo.GetEmptyNumbersDataTable();
        }
    }
}
