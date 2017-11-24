using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyst.DBAccess.Contexts;
using System.Collections.Concurrent;
using log4net;
using Analyst.Domain.Edgar;

namespace Analyst.Services.EdgarDatasetServices
{
    public interface IEdgarDatasetPresentationService : IEdgarFileService<EdgarDatasetPresentation>
    {
        ConcurrentDictionary<string, int> Nums { get; set; }
        ConcurrentDictionary<string, int> Renders { get; set; }
        ConcurrentDictionary<string, int> Subs { get; set; }
        ConcurrentDictionary<string, int> Tags { get; set; }
        ConcurrentDictionary<string, int> Texts { get; set; }
    }
    public class EdgarDatasetPresentationService : EdgarFileService<EdgarDatasetPresentation>, IEdgarDatasetPresentationService
    {
        public ConcurrentDictionary<string, int> Renders { get; set; }
        public ConcurrentDictionary<string, int> Subs { get; set; }
        public ConcurrentDictionary<string, int> Tags { get; set; }

        public ConcurrentDictionary<string, int> Nums { get; set; }
        public ConcurrentDictionary<string, int> Texts { get; set; }

        private readonly ILog log;
        protected override ILog Log
        {
            get
            {
                return log;
            }
        }
        public EdgarDatasetPresentationService()
        {
            log = log4net.LogManager.GetLogger(this.GetType().Name);
        }
        public override void Add(IAnalystRepository repo, EdgarDataset dataset, EdgarDatasetPresentation file)
        {
            repo.Add(dataset,file);
        }

        public override EdgarDatasetPresentation Parse(IAnalystRepository repository, List<string> fieldNames, List<string> fields, int lineNumber, ConcurrentDictionary<string, int> existing)
        {
            /*
            adsh	report	line	stmt	inpth	rfile	tag	version	prole	plabel	negating
            0001163302-16-000148	1	4	CP	0	H	DocumentFiscalYearFocus	dei/2014	terseLabel	Document Fiscal Year Focus	0
            0001163302-16-000148	1	3	CP	0	H	DocumentPeriodEndDate	dei/2014	terseLabel	Document Period End Date	0
            ...
            */
            try
            {
                EdgarDatasetPresentation pre = new EdgarDatasetPresentation();
                string adsh = fields[fieldNames.IndexOf("adsh")];
                pre.SubmissionId = Subs[adsh];
                string report = fields[fieldNames.IndexOf("report")];
                pre.RenderId = Renders[adsh + report];
                if (pre.Render != null)
                    pre.RenderId = pre.Render.Id;
                pre.Line = Convert.ToInt32(fields[fieldNames.IndexOf("line")]);
                pre.FinancialStatement = fields[fieldNames.IndexOf("stmt")];
                pre.Inpth = fields[fieldNames.IndexOf("inpth")] == "1";
                pre.RenderFile = fields[fieldNames.IndexOf("rfile")][0];
                string tag = fields[fieldNames.IndexOf("tag")];
                string version = fields[fieldNames.IndexOf("version")];
                pre.TagId = Tags[tag + version];
                pre.prole = fields[fieldNames.IndexOf("prole")];
                pre.PreferredLabel = fields[fieldNames.IndexOf("plabel")];
                pre.Negating = !(fields[fieldNames.IndexOf("negating")] == "0");
                pre.LineNumber = lineNumber;

                string key = adsh + tag + version;
                if (Nums.ContainsKey(key))
                    pre.NumberId = Nums[key];
                else
                    pre.ADSH_Tag_Version = adsh + "|" + tag + "|" + version;

                if (Texts.ContainsKey(key))
                    pre.TextId = Texts[key];
                else
                    pre.ADSH_Tag_Version = adsh + "|" + tag + "|" + version;


                return pre;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public override IList<EdgarTuple> GetKeys(IAnalystRepository repository, int datasetId)
        {
            throw new NotImplementedException();
        }
    }
}
