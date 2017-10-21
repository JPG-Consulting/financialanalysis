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
            string cacheFolder = ConfigurationManager.AppSettings["cache_folder"];
            string filepath = cacheFolder + ds.RelativePath.Replace("/", "\\").Replace(".zip", "") + "\\tag.tsv";
            StreamReader sr = File.OpenText(filepath);
            string header = sr.ReadLine();//header
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                EdgarDatasetTag tag = ParseTag(header, line);
                Repository.Save(ds, tag);
            }
            sr.Close();
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

            sub.Registrant = ParseRegistrant(fields[fieldNames.IndexOf("cik")], fieldNames, fields);

            sub.Form =Repository.GetSECForm(fields[fieldNames.IndexOf("form")]);

            string period = fields[fieldNames.IndexOf("period")];
            sub.Period = new DateTime(int.Parse(period.Substring(0, 4)), int.Parse(period.Substring(4, 2)), int.Parse(period.Substring(6, 2)));

            sub.Detail = fields[fieldNames.IndexOf("period")] == "1";

            sub.XBRLInstance = fields[fieldNames.IndexOf("instance")];

            sub.NumberOfCIKs = int.Parse(fields[fieldNames.IndexOf("nciks")]);

            string value = fields[fieldNames.IndexOf("aciks")];
            sub.AdditionalCIKs = String.IsNullOrEmpty(value) ? null : value;

            value = fields[fieldNames.IndexOf("pubfloatusd")];
            sub.PubFloatUSD = string.IsNullOrEmpty(value) ? (float?) null :float.Parse(value);

            string floatdate = fields[fieldNames.IndexOf("floatdate")];
            sub.FloatDate = String.IsNullOrEmpty(floatdate)? (DateTime?) null: new DateTime(int.Parse(floatdate.Substring(0, 4)), int.Parse(floatdate.Substring(4, 2)), int.Parse(floatdate.Substring(6, 2)));

            value = fields[fieldNames.IndexOf("floataxis")];
            sub.FloatAxis = String.IsNullOrEmpty(value) ? null : value;

            value = fields[fieldNames.IndexOf("floatmems")];
            sub.FloatMems = string.IsNullOrEmpty(value)? (int?)null : int.Parse(value);

            return sub;
        }


        private Registrant ParseRegistrant(string cik, List<string> fieldNames, List<string> fields)
        {
            Registrant r = Repository.GetRegistrant(cik);
            if(r==null)
            {
                r = new Company();//TODO: no todos los registrantes son companias, algunos son mutual fund
                r. CIK = int.Parse(cik);
                r.Name = fields[fieldNames.IndexOf("name")];
                string value = fields[fieldNames.IndexOf("sic")];
                r.SIC = string.IsNullOrEmpty(value) ? null : Repository.GetSIC(value);
                value = fields[fieldNames.IndexOf("countryba")];
                r.CountryBA = String.IsNullOrEmpty(value) ? null : value;
                value = fields[fieldNames.IndexOf("cityba")];
                r.CityBA = String.IsNullOrEmpty(value) ? null : value; 
                value = fields[fieldNames.IndexOf("countryinc")];
                r.CountryInc = String.IsNullOrEmpty(value) ? null : value;
                value = fields[fieldNames.IndexOf("ein")];
                r.EIN = string.IsNullOrEmpty(value)? (int?)null: int.Parse(value);
                r.AFS = fields[fieldNames.IndexOf("afs")];
                r.WKSI = fields[fieldNames.IndexOf("wksi")] == "1";
                value = fields[fieldNames.IndexOf("fye")];
                r.FYE = string.IsNullOrEmpty(value) ? null : value;
                Repository.AddRegistrant(r);
            }
            return r;
        }

        private EdgarDatasetTag ParseTag(string header, string line)
        {
            /*
            tag	version	custom	abstract	datatype	iord	crdr	tlabel	doc
            AccountsPayableCurrent	us-gaap/2015	0	0	monetary	I	C	Accounts Payable, Current	Carrying value as of the balance sheet date of liabilities incurred (and for which invoices have typically been received) and payable to vendors for goods and services received that are used in an entity's business. Used to reflect the current portion of the liabilities (due within one year or within the normal operating cycle if longer).
            AccountsPayableRelatedPartiesCurrent	us-gaap/2015	0	0	monetary	I	C	Accounts Payable, Related Parties, Current	Amount for accounts payable to related parties. Used to reflect the current portion of the liabilities (due within one year or within the normal operating cycle if longer).
            */
            List<string> fieldNames = header.Split('\t').ToList();
            List<string> fields = line.Split('\t').ToList();

            string strTag = fields[fieldNames.IndexOf("tag")];
            string version = fields[fieldNames.IndexOf("version")];
            EdgarDatasetTag tag = Repository.GetTag(strTag,version);
            if(tag == null)
            {
                tag = new EdgarDatasetTag();
                tag.Tag = strTag;
                tag.Version = version;
                string value = fields[fieldNames.IndexOf("custom")];
                tag.Custom = value == "1" ? true: false;
                value = fields[fieldNames.IndexOf("abstract")];
                tag.Abstract = value == "1" ? true : false;
                value = fields[fieldNames.IndexOf("datatype")];
                tag.Datatype = string.IsNullOrEmpty(value) ? null : value;
                value = fields[fieldNames.IndexOf("iord")];
                tag.Iord = string.IsNullOrEmpty(value) ? (char?)null : value[0];
                value = fields[fieldNames.IndexOf("crdr")];
                tag.Crdr = string.IsNullOrEmpty(value) ? (char?)null : value[0];
                value = fields[fieldNames.IndexOf("tlabel")];
                tag.Tlabel = string.IsNullOrEmpty(value) ? null : value;
                value = fields[fieldNames.IndexOf("doc")];
                tag.Doc = string.IsNullOrEmpty(value) ? null : value;
            }
            return tag;
        }
    }
}
