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

namespace Analyst.Services.EdgarDatasetServices
{
    public interface IEdgarDatasetNumService : IEdgarFileService<EdgarDatasetNumber>
    {
        ConcurrentDictionary<string, EdgarDatasetDimension> Dimensions { get; set; }
        ConcurrentDictionary<string, EdgarDatasetSubmission> Submissions { get; set; }
        ConcurrentDictionary<string, EdgarDatasetTag> Tags { get; set; }
    }
    public class EdgarDatasetNumService:EdgarFileService<EdgarDatasetNumber>, IEdgarDatasetNumService
    {
        
        public ConcurrentDictionary<string, EdgarDatasetSubmission> Submissions { get; set; }
        public ConcurrentDictionary<string, EdgarDatasetTag> Tags { get; set; }
        public ConcurrentDictionary<string, EdgarDatasetDimension> Dimensions { get; set; }

        
        public override void Add(IAnalystRepository repo, EdgarDataset dataset, EdgarDatasetNumber file)
        {
            file.Submission = Submissions[file.ADSH];
            file.Tag = Tags[file.TagCompoundKey];
            file.Dimension = Dimensions[file.DimensionStr];
            repo.Add(dataset, file);
        }

        public override EdgarDatasetNumber Parse(IAnalystRepository repository, List<string> fieldNames, List<string> fields, int lineNumber)
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
            number.DDate = new DateTime(int.Parse(value.Substring(0, 4)), int.Parse(value.Substring(4, 2)), int.Parse(value.Substring(6, 2)));
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
                number.Value = double.Parse(value);
            number.FootNote = fields[fieldNames.IndexOf("footnote")];
            value = fields[fieldNames.IndexOf("footlen")];
            number.FootLength = Convert.ToInt16(value);
            value = fields[fieldNames.IndexOf("dimn")];
            number.NumberOfDimensions = Convert.ToInt16(value);
            number.CoRegistrant = fields[fieldNames.IndexOf("coreg")];
            value = fields[fieldNames.IndexOf("durp")];
            number.durp = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
            value = fields[fieldNames.IndexOf("datp")];
            number.datp = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
            value = fields[fieldNames.IndexOf("dcml")];
            number.Decimals = Convert.ToInt32(value);
            return number;
        }
    }
}
