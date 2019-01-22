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

namespace Analyst.Services.EdgarDatasetServices.LineByLineProcessStrategy
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
        public override void Add(IAnalystEdgarDatasetsRepository repo, EdgarDataset dataset, EdgarDatasetNumber file)
        {
            file.SubmissionId = Submissions[file.ADSH];
            file.TagId = Tags[file.TagCompoundKey];
            file.DimensionId = Dimensions[file.DimensionStr];
            repo.Add(dataset, file);
        }

        public override EdgarDatasetNumber Parse(IAnalystEdgarDatasetsRepository repository, List<string> fieldNames, List<string> fields, int lineNumber)
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
            number.durp = float.Parse(value, CultureInfo.GetCultureInfo("en-us").NumberFormat);
            value = fields[fieldNames.IndexOf("datp")];
            number.datp = float.Parse(value, CultureInfo.GetCultureInfo("en-us").NumberFormat);
            value = fields[fieldNames.IndexOf("dcml")];
            number.Decimals = Convert.ToInt32(value);

            number.LineNumber = lineNumber;

            return number;
        }

        public override IList<EdgarTuple> GetKeys(IAnalystEdgarDatasetsRepository repository, int datasetId)
        {
            return repository.GetNumberKeys(datasetId);
        }



        public override List<int> GetMissingLinesByTable(IAnalystEdgarDatasetsRepository repo, int datasetId, int totalLines)
        {
            return repo.GetMissingLines(datasetId,"EdgarDatasetNumbers", totalLines);
        }

        
    }
}
