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

namespace Analyst.Services.EdgarServices
{
    public interface INumService
    {
        void ProcessNums(EdgarTaskState state, ConcurrentDictionary<string, EdgarDatasetSubmissions> subs, ConcurrentDictionary<string, EdgarDatasetTag> tags, ConcurrentDictionary<string, EdgarDatasetDimension> dims);
    }
    public class NumService:INumService
    {
        public void ProcessNums(EdgarTaskState state, ConcurrentDictionary<string, EdgarDatasetSubmissions> subs, ConcurrentDictionary<string, EdgarDatasetTag> tags, ConcurrentDictionary<string, EdgarDatasetDimension> dims)
        {
            string cacheFolder = ConfigurationManager.AppSettings["cache_folder"];
            string filepath = cacheFolder + state.Dataset.RelativePath.Replace("/", "\\").Replace(".zip", "") + "\\num.tsv";
            string[] allLines = File.ReadAllLines(filepath);
            string header = allLines[0];
            state.Dataset.TotalNumbers = allLines.Length-1;
            state.DatasetSharedRepo.UpdateEdgarDataset(state.Dataset, "TotalNumbers");
            IAnalystRepository repo = new AnalystRepository(new AnalystContext());
            ProcessRange(state,subs,tags,dims, new Tuple<int,int>(1, allLines.Length), allLines, header, repo);
        }

        private void ProcessRange(EdgarTaskState state, ConcurrentDictionary<string, EdgarDatasetSubmissions> subs, ConcurrentDictionary<string, EdgarDatasetTag> tags,ConcurrentDictionary<string,EdgarDatasetDimension> dims, Tuple<int, int> range, string[] allLines, string header, IAnalystRepository repo)
        {
            for (int i = range.Item1; i < range.Item2; i++)
            {
                string line = allLines[i];


                EdgarDatasetNumber number = repo.GetNumber(state.Dataset.Id, i);
                if (number == null)
                {
                    number = ParseNum(repo, header, line, i);
                    number.Submission = subs[number.ADSH];
                    number.Tag = tags[number.TagCompoundKey];
                    number.Dimension = dims[number.DimensionStr];
                    repo.AddNumber(state.Dataset, number);
                }
            }
        }

        private EdgarDatasetNumber ParseNum(IAnalystRepository repo, string header, string line,int linenumber)
        {
            /*
            Ejemplo
            adsh	tag	version	ddate	qtrs	uom	dimh	iprx	value	footnote	footlen	dimn	coreg	durp	datp	dcml
            0000846913-16-000146	ShareBasedCompensationArrangementByShareBasedPaymentAwardOptionsVestedAndExpectedToVestExercisableAggregateIntrinsicValue	us-gaap/2015	20160930	0	USD	0x00000000	0	0.0000		0	0		0.0	0.0	-3
            0000846913-16-000146	WeightedAverageNumberOfSharesRestrictedStock	us-gaap/2015	20150930	1	shares	0x00000000	0	0.0000		0	0		0.0027400255	0.0	-3
            */
            EdgarDatasetNumber number = new EdgarDatasetNumber();
            List<string> fieldNames = header.Split('\t').ToList();
            List<string> fields = line.Split('\t').ToList();
            String value = "";
            number.LineNumber = linenumber;
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
