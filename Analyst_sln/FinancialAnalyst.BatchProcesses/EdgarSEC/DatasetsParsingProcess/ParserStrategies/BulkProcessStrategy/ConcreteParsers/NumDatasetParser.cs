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
    public class NumDatasetParser: BulkEdgarDatasetParser<EdgarDatasetNumber>, INumDatasetParser
    {
        
        public ConcurrentDictionary<string, int> Submissions { get; set; }
        public ConcurrentDictionary<string, int> Tags { get; set; }
        public ConcurrentDictionary<string, int> Dimensions { get; set; }

        protected override DatasetsTables RelatedTable { get { return DatasetsTables.Numbers; } }

        private readonly ILog log;
        protected override ILog Log
        {
            get
            {
                return log;
            }
        }
        public NumDatasetParser()
        {
            log = log4net.LogManager.GetLogger(this.GetType());
        }

        public override IList<EdgarTuple> GetKeys(IEdgarDatasetsRepository repository, int datasetId)
        {
            return repository.GetNumberKeys(datasetId);
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

            string adsh = fields[fieldNames.IndexOf("adsh")];

            string tag = fields[fieldNames.IndexOf("tag")];

            string version = fields[fieldNames.IndexOf("version")];

            value = fields[fieldNames.IndexOf("ddate")];
            dr["DatavalueEnddate"] = new DateTime(int.Parse(value.Substring(0, 4)), int.Parse(value.Substring(4, 2)), int.Parse(value.Substring(6, 2)));

            value = fields[fieldNames.IndexOf("qtrs")];
            dr["CountOfNumberOfQuarters"] = Convert.ToInt32(value);

            dr["UnitOfMeasure"] = fields[fieldNames.IndexOf("uom")];

            string dimh = fields[fieldNames.IndexOf("dimh")];
            //if (!Dimensions.ContainsKey(dimh)) throw new KeyNotFoundException("Dimensions[" + dimh + "]");
            dr["DimensionId"] = Dimensions[dimh];

            value = fields[fieldNames.IndexOf("iprx")];
            if (string.IsNullOrEmpty(value))
                dr["IPRX"] = DBNull.Value;
            else
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
            if (string.IsNullOrEmpty(value))
                dr["durp"] = DBNull.Value;
            else
                dr["durp"] = float.Parse(value, CultureInfo.GetCultureInfo("en-us").NumberFormat);

            value = fields[fieldNames.IndexOf("datp")];
            if (string.IsNullOrEmpty(value))
                dr["datp"] = DBNull.Value;
            else
                dr["datp"] = float.Parse(value, CultureInfo.GetCultureInfo("en-us").NumberFormat);

            value = fields[fieldNames.IndexOf("dcml")];
            if (string.IsNullOrEmpty(value))
                dr["Decimals"] = DBNull.Value;
            else
                dr["Decimals"] = Convert.ToInt32(value);
            
            dr["DatasetId"] = edgarDatasetId;

            dr["LineNumber"] = lineNumber;



            //if (!Submissions.ContainsKey(adsh)) throw new KeyNotFoundException("Submissions[" + adsh + "]");
            dr["SubmissionId"] = Submissions[adsh];

            //if (!Tags.ContainsKey(tag + version)) throw new KeyNotFoundException("Tags[" + tag + version + "]");
            dr["TagId"] = Tags[tag + version];

            

        }
    }
}
