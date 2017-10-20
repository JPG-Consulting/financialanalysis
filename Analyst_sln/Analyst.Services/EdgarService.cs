using Analyst.DBAccess;
using Analyst.DBAccess.Contexts;
using Analyst.Domain;
using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyst.Domain.Edgar;
using System.Configuration;
using System.IO;

namespace Analyst.Services
{
    public interface IEdgarService
    {
        List<EdgarDataset> GetDatasets();
        List<SECForm> GetSECForms();
        List<SIC> GetSICs();
        EdgarDataset ProcessDataset(int id);
        EdgarDataset GetDataset(int id);
    }

    public class EdgarService: IEdgarService
    {


        private IAnalystRepository _repo;
        //TODO: Usar Unity para Dependency Injection
        public IAnalystRepository Repository
        {
            get
            {
                if(_repo == null)
                    _repo = new AnalystRepository();
                return _repo;
            }
            set { }
        }

        public List<EdgarDataset> GetDatasets()
        {
            InitialLoader.LoadInitialData(Repository);
            List<EdgarDataset> datasets = Repository.GetDatasets();
            return datasets;
        }

        public EdgarDataset GetDataset(int id)
        {
            EdgarDataset ds = Repository.GetDataset(id);
            return ds;
        }

        public List<SECForm> GetSECForms()
        {
            InitialLoader.LoadInitialData(Repository);
            List<SECForm> forms = Repository.GetSECForms();
            return forms;
        }

        public List<SIC> GetSICs()
        {
            InitialLoader.LoadInitialData(Repository);
            List<SIC> sics = Repository.GetSICs();
            return sics;
        }


        public EdgarDataset ProcessDataset(int id)
        {
            EdgarDataset ds = Repository.GetDataset(id);
            ProcessSubmissions(ds);
            ProcessTags(ds);
            return ds;
        }

        private void ProcessTags(EdgarDataset ds)
        {
            throw new NotImplementedException();
        }

        private void ProcessSubmissions(EdgarDataset ds)
        {
            try
            {
                string cacheFolder = ConfigurationManager.AppSettings["cache_folder"];
                string filepath = cacheFolder + ds.RelativePath.Replace("/", "\\").Replace(".zip", "") + "\\sub.tsv";
                StreamReader sr = File.OpenText(filepath);
                string header = sr.ReadLine();//header
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    EdgarDatasetSubmissions sub = ParseSub(header, line);
                    Repository.Save(ds, sub);
                }
                sr.Close();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public EdgarDatasetSubmissions ParseSub(string header, string line)
        {
            EdgarDatasetSubmissions sub = new EdgarDatasetSubmissions();

            //Ejemplo
            //adsh	cik	name	sic	countryba	stprba	cityba	zipba	bas1	bas2	baph	countryma	stprma	cityma	zipma	mas1	mas2	countryinc	stprinc	ein	former	changed	afs	wksi	fye	form	period	fy	fp	filed	accepted	prevrpt	detail	instance	nciks	aciks	pubfloatusd	floatdate	floataxis	floatmems
            //0000002178 - 16 - 000103    2178    ADAMS RESOURCES &ENERGY, INC.  5172    US TX  HOUSTON 77027   17 S.BRIAR HOLLOW LN.      7138813600  US TX  HOUSTON 77001   P O BOX 844     US DE  741753147   ADAMS RESOURCES &ENERGY INC    19920703    2 - ACC   0   1231    10 - Q    20160930    2016    Q3  20161109    2016 - 11 - 09 12:49:00.0   0   1   ae - 20160930.xml 1

            List<string> fieldNames = header.Split('\t').ToList();
            List<string> fields = line.Split('\t').ToList();
            sub.ADSH = fields[fieldNames.IndexOf("adsh")];
            sub.Registrant = ProcessRegistrant(fields[fieldNames.IndexOf("cik")], fieldNames, fields);
            sub.Form =Repository.GetSECForm(fields[fieldNames.IndexOf("form")]);
            string period = fields[fieldNames.IndexOf("period")];
            sub.Period = new DateTime(int.Parse(period.Substring(0, 4)), int.Parse(period.Substring(4, 2)), int.Parse(period.Substring(6, 2)));
            sub.Detail = fields[fieldNames.IndexOf("period")] == "1";
            sub.XBRLInstance = fields[fieldNames.IndexOf("instance")];
            sub.NumberOfCIKs = int.Parse(fields[fieldNames.IndexOf("nciks")]);
            string value = fields[fieldNames.IndexOf("aciks")];
            sub.AdditionalCIKs = String.IsNullOrEmpty(value) ? null : value;

            value = fields[fieldNames.IndexOf("pubfloatusd")];
            if (string.IsNullOrEmpty(value)) sub.PubFloatUSD = null;
            else sub.PubFloatUSD = float.Parse(value);

            string floatdate = fields[fieldNames.IndexOf("floatdate")];
            if (String.IsNullOrEmpty(floatdate))
                sub.FloatDate = null;
            else
                sub.FloatDate = new DateTime(int.Parse(floatdate.Substring(0, 4)), int.Parse(floatdate.Substring(4, 2)), int.Parse(floatdate.Substring(6, 2)));

            value = fields[fieldNames.IndexOf("floataxis")];
            sub.FloatAxis = String.IsNullOrEmpty(value) ? null : value;

            value = fields[fieldNames.IndexOf("floatmems")];
            if (string.IsNullOrEmpty(value)) sub.FloatMems = null;
            else sub.FloatMems = int.Parse(value);

            return sub;
        }


        private Registrant ProcessRegistrant(string cik, List<string> fieldNames, List<string> fields)
        {
            Registrant r = Repository.GetRegistrant(cik);
            if(r==null)
            {
                r = new Company();//TODO: no todos los registrantes son companias, algunos son mutual fund
                r. CIK = int.Parse(cik);
                r.Name = fields[fieldNames.IndexOf("name")];
                string value = fields[fieldNames.IndexOf("sic")];
                if (string.IsNullOrEmpty(value))
                    r.SIC = null;
                else
                    r.SIC = Repository.GetSIC(value);
                value = fields[fieldNames.IndexOf("countryba")];
                r.CountryBA = String.IsNullOrEmpty(value) ? null : value;
                value = fields[fieldNames.IndexOf("cityba")];
                r.CityBA = String.IsNullOrEmpty(value) ? null : value; 
                value = fields[fieldNames.IndexOf("countryinc")];
                r.CountryInc = String.IsNullOrEmpty(value) ? null : value;
                value = fields[fieldNames.IndexOf("ein")];
                if (string.IsNullOrEmpty(value))
                    r.EIN = null;
                else
                    r.EIN = int.Parse(value);
                r.AFS = fields[fieldNames.IndexOf("afs")];
                r.WKSI = fields[fieldNames.IndexOf("wksi")] == "1";
                value = fields[fieldNames.IndexOf("fye")];
                r.FYE = string.IsNullOrEmpty(value) ? null : value;
                Repository.AddRegistrant(r);
            }
            return r;
        }
    }
}
