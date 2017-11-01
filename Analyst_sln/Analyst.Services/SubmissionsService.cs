using Analyst.DBAccess.Contexts;
using Analyst.Domain.Edgar;
using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Analyst.Services.EdgarService;

namespace Analyst.Services
{
    public interface ISubmissionService
    {
        void ProcessSubmissions(EdgarTaskState ds);

    }
    public class SubmissionsService: ISubmissionService
    {

        public void ProcessSubmissions(EdgarTaskState state)
        {
            try
            {
                string cacheFolder = ConfigurationManager.AppSettings["cache_folder"];
                string filepath = cacheFolder + state.Dataset.RelativePath.Replace("/", "\\").Replace(".zip", "") + "\\sub.tsv";
                StreamReader sr = File.OpenText(filepath);
                string header = sr.ReadLine();
                using (IAnalystRepository repository = new AnalystRepository(new AnalystContext()))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        EdgarDatasetSubmissions sub = ParseSub(repository, header, line);
                        repository.Save(state.Dataset, sub);
                    }
                }
                sr.Close();
                state.Result = true;
            }
            catch (Exception ex)
            {
                state.Result = false;
                state.Exception= ex;
            }
        }

        private EdgarDatasetSubmissions ParseSub(IAnalystRepository repository,string header, string line)
        {
            EdgarDatasetSubmissions sub = new EdgarDatasetSubmissions();

            //Example
            //adsh	cik	name	sic	countryba	stprba	cityba	zipba	bas1	bas2	baph	countryma	stprma	cityma	zipma	mas1	mas2	countryinc	stprinc	ein	former	changed	afs	wksi	fye	form	period	fy	fp	filed	accepted	prevrpt	detail	instance	nciks	aciks	pubfloatusd	floatdate	floataxis	floatmems
            //0000002178 - 16 - 000103    2178    ADAMS RESOURCES &ENERGY, INC.  5172    US TX  HOUSTON 77027   17 S.BRIAR HOLLOW LN.      7138813600  US TX  HOUSTON 77001   P O BOX 844     US DE  741753147   ADAMS RESOURCES &ENERGY INC    19920703    2 - ACC   0   1231    10 - Q    20160930    2016    Q3  20161109    2016 - 11 - 09 12:49:00.0   0   1   ae - 20160930.xml 1

            List<string> fieldNames = header.Split('\t').ToList();
            List<string> fields = line.Split('\t').ToList();

            sub.ADSH = fields[fieldNames.IndexOf("adsh")];

            sub.Registrant = ParseRegistrant(repository,fields[fieldNames.IndexOf("cik")], fieldNames, fields);

            sub.Form = repository.GetSECForm(fields[fieldNames.IndexOf("form")]);

            string period = fields[fieldNames.IndexOf("period")];
            sub.Period = new DateTime(int.Parse(period.Substring(0, 4)), int.Parse(period.Substring(4, 2)), int.Parse(period.Substring(6, 2)));

            sub.Detail = fields[fieldNames.IndexOf("period")] == "1";

            sub.XBRLInstance = fields[fieldNames.IndexOf("instance")];

            sub.NumberOfCIKs = int.Parse(fields[fieldNames.IndexOf("nciks")]);

            string value = fields[fieldNames.IndexOf("aciks")];
            sub.AdditionalCIKs = String.IsNullOrEmpty(value) ? null : value;

            value = fields[fieldNames.IndexOf("pubfloatusd")];
            sub.PubFloatUSD = string.IsNullOrEmpty(value) ? (float?)null : float.Parse(value);

            string floatdate = fields[fieldNames.IndexOf("floatdate")];
            sub.FloatDate = String.IsNullOrEmpty(floatdate) ? (DateTime?)null : new DateTime(int.Parse(floatdate.Substring(0, 4)), int.Parse(floatdate.Substring(4, 2)), int.Parse(floatdate.Substring(6, 2)));

            value = fields[fieldNames.IndexOf("floataxis")];
            sub.FloatAxis = String.IsNullOrEmpty(value) ? null : value;

            value = fields[fieldNames.IndexOf("floatmems")];
            sub.FloatMems = string.IsNullOrEmpty(value) ? (int?)null : int.Parse(value);

            return sub;
        }


        private Registrant ParseRegistrant(IAnalystRepository repository,string cik, List<string> fieldNames, List<string> fields)
        {
            Registrant r = repository.GetRegistrant(cik);
            if (r == null)
            {
                r = new Company();//TODO: no todos los registrantes son companias, algunos son mutual fund
                r.CIK = int.Parse(cik);
                r.Name = fields[fieldNames.IndexOf("name")];
                string value = fields[fieldNames.IndexOf("sic")];
                r.SIC = string.IsNullOrEmpty(value) ? null : repository.GetSIC(value);
                value = fields[fieldNames.IndexOf("countryba")];
                r.CountryBA = String.IsNullOrEmpty(value) ? null : value;
                value = fields[fieldNames.IndexOf("cityba")];
                r.CityBA = String.IsNullOrEmpty(value) ? null : value;
                value = fields[fieldNames.IndexOf("countryinc")];
                r.CountryInc = String.IsNullOrEmpty(value) ? null : value;
                value = fields[fieldNames.IndexOf("ein")];
                r.EIN = string.IsNullOrEmpty(value) ? (int?)null : int.Parse(value);
                r.AFS = fields[fieldNames.IndexOf("afs")];
                r.WKSI = fields[fieldNames.IndexOf("wksi")] == "1";
                value = fields[fieldNames.IndexOf("fye")];
                r.FYE = string.IsNullOrEmpty(value) ? null : value;
                repository.AddRegistrant(r);
            }
            return r;
        }


    }
}
