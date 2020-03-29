using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using log4net;
using FinancialAnalyst.Common.Entities.EdgarSEC;
using FinancialAnalyst.Common.Entities.EdgarSEC.Datasets;
using FinancialAnalyst.Common.Entities.EdgarSEC.Repositories;
using FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess.ParserStrategies.Interfaces.InterfacesForConcreteParsers;
using System.Globalization;

namespace FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess.ParserStrategies.LineByLineProcessStrategy.ConcreteParsers
{
    public class NumDatasetParser: LineByLineEdgarDatasetParser<EdgarDatasetNumber>, INumDatasetParser
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
        public override void Add(IEdgarDatasetsRepository repo, EdgarDataset dataset, EdgarDatasetNumber file)
        {
            file.SubmissionId = Submissions[file.ADSH];
            file.TagId = Tags[file.TagCompoundKey];
            file.DimensionId = Dimensions[file.DimensionStr];
            repo.Add(dataset, file);
        }

        public override EdgarDatasetNumber Parse(IEdgarDatasetsRepository repository, List<string> fieldNames, List<string> fields, int lineNumber)
        {
            /*
            Ejemplo
            adsh	tag	version	ddate	qtrs	uom	dimh	iprx	value	footnote	footlen	dimn	coreg	durp	datp	dcml
            0000846913-16-000146	ShareBasedCompensationArrangementByShareBasedPaymentAwardOptionsVestedAndExpectedToVestExercisableAggregateIntrinsicValue	us-gaap/2015	20160930	0	USD	0x00000000	0	0.0000		0	0		0.0	0.0	-3
            0000846913-16-000146	WeightedAverageNumberOfSharesRestrictedStock	us-gaap/2015	20150930	1	shares	0x00000000	0	0.0000		0	0		0.0027400255	0.0	-3
            */
            EdgarDatasetNumber number = new EdgarDatasetNumber();
            String value = "";
            number.LineNumber = lineNumber;
            number.ADSH = fields[fieldNames.IndexOf("adsh")];
            number.TagStr = fields[fieldNames.IndexOf("tag")];
            number.Version = fields[fieldNames.IndexOf("version")];
            value = fields[fieldNames.IndexOf("ddate")];
            number.DatavalueEnddate = new DateTime(int.Parse(value.Substring(0, 4)), int.Parse(value.Substring(4, 2)), int.Parse(value.Substring(6, 2)));
            value = fields[fieldNames.IndexOf("qtrs")];
            number.CountOfNumberOfQuarters = Convert.ToInt32(value);
            number.UnitOfMeasure = fields[fieldNames.IndexOf("uom")];
            number.DimensionStr = fields[fieldNames.IndexOf("dimh")];
            value = fields[fieldNames.IndexOf("iprx")];
            number.IPRX = Convert.ToInt16(value);
            value = fields[fieldNames.IndexOf("value")];
            if (String.IsNullOrEmpty(value))
                number.Value = null;
            else
                number.Value = double.Parse(value, CultureInfo.GetCultureInfo("en-us").NumberFormat);
            value = fields[fieldNames.IndexOf("footnote")];
            if (string.IsNullOrEmpty(value))
                number.FootNote = null;
            else
                number.FootNote = value;
            value = fields[fieldNames.IndexOf("footlen")];
            number.FootLength = Convert.ToInt16(value);
            value = fields[fieldNames.IndexOf("dimn")];
            number.NumberOfDimensions = Convert.ToInt16(value);
            value = fields[fieldNames.IndexOf("coreg")];
            if (string.IsNullOrEmpty(value))
                number.CoRegistrant = null;
            else
                number.CoRegistrant = value;
            value = fields[fieldNames.IndexOf("durp")];
            number.Durp = float.Parse(value, CultureInfo.GetCultureInfo("en-us").NumberFormat);
            value = fields[fieldNames.IndexOf("datp")];
            number.Datp = float.Parse(value, CultureInfo.GetCultureInfo("en-us").NumberFormat);
            value = fields[fieldNames.IndexOf("dcml")];
            number.Decimals = Convert.ToInt32(value);

            number.LineNumber = lineNumber;

            return number;
        }

        public override IList<EdgarTuple> GetKeys(IEdgarDatasetsRepository repository, int datasetId)
        {
            return repository.GetNumberKeys(datasetId);
        }
        
    }
}
