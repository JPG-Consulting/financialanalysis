using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using log4net;
using FinancialAnalyst.Common.Entities.EdgarSEC;
using FinancialAnalyst.Common.Entities.EdgarSEC.Datasets;
using FinancialAnalyst.Common.Entities.EdgarSEC.Repositories;
using FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess.ParserStrategies.Interfaces.InterfacesForConcreteParsers;
using System.Text;
using FinancialAnalyst.Common.Exceptions.EdgarSEC;

namespace FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess.ParserStrategies.LineByLineProcessStrategy.ConcreteParsers
{

    public class PresentationDatasetParser : LineByLineEdgarDatasetParser<EdgarDatasetPresentation>, IPresentationDatasetParser
    {
        public ConcurrentDictionary<string, int> Renders { get; set; }
        public ConcurrentDictionary<string, int> Subs { get; set; }
        public ConcurrentDictionary<string, int> Tags { get; set; }

        public ConcurrentDictionary<string, int> Nums { get; set; }
        public ConcurrentDictionary<string, int> Texts { get; set; }

        protected override DatasetsTables RelatedTable { get { return DatasetsTables.Presentations; } }

        private readonly ILog log;
        protected override ILog Log
        {
            get
            {
                return log;
            }
        }
        public PresentationDatasetParser()
        {
            log = log4net.LogManager.GetLogger(this.GetType());
        }
        public override void Add(IEdgarDatasetsRepository repo, EdgarDataset dataset, EdgarDatasetPresentation file)
        {
            repo.Add(dataset,file);
        }

        public override EdgarDatasetPresentation Parse(IEdgarDatasetsRepository repository, List<string> fieldNames, List<string> fields, int lineNumber)
        {
            /*
            adsh	report	line	stmt	inpth	rfile	tag	version	prole	plabel	negating
            0001163302-16-000148	1	4	CP	0	H	DocumentFiscalYearFocus	dei/2014	terseLabel	Document Fiscal Year Focus	0
            0001163302-16-000148	1	3	CP	0	H	DocumentPeriodEndDate	dei/2014	terseLabel	Document Period End Date	0
            0001125345-17-000041	6	1	CF	0	H	NetCashProvidedByUsedInOperatingActivitiesContinuingOperationsAbstract	us-gaap/2015	terseLabel	Operating activities	0
            0001104659-17-016575	111	6		0	H	NetCashProvidedByUsedInOperatingActivitiesContinuingOperationsAbstract	us-gaap/2015	terseLabel	Cash flows from operating activities:	0
            ...
            */
            try
            {
                EdgarDatasetPresentation pre;
                string adsh = fields[fieldNames.IndexOf("adsh")];
                string line = fields[fieldNames.IndexOf("line")];
                string report = fields[fieldNames.IndexOf("report")];
                //select S.ADSH + CAST(p.ReportNumber as varchar) + cast(p.Line as varchar) [key] ...
                //string key = adsh + report + line;

                pre = new EdgarDatasetPresentation();
                pre.SubmissionId = Subs[adsh];
                pre.ReportNumber = Convert.ToInt32(report);
                pre.RenderId = Renders[adsh + report];
                pre.Line = Convert.ToInt32(line);
                pre.FinancialStatement = fields[fieldNames.IndexOf("stmt")];
                pre.Inpth = fields[fieldNames.IndexOf("inpth")] == "1";
                int index = fieldNames.IndexOf("rfile");
                if (index >= 0)
                {
                    pre.RenderFile = fields[fieldNames.IndexOf("rfile")][0];
                    pre.RenderFileStr = fields[fieldNames.IndexOf("rfile")];
                }
                else
                {
                    pre.RenderFileStr = null;
                    pre.RenderFile = char.MinValue;
                }
                string tag = fields[fieldNames.IndexOf("tag")];
                string version = fields[fieldNames.IndexOf("version")];
                if(Tags.ContainsKey(tag+version))
                    pre.TagId = Tags[tag + version];
                else
                {
                    string tag2 = Encoding.GetEncoding(1252).GetString(Encoding.GetEncoding("iso-8859-7").GetBytes(tag));
                    if (Tags.ContainsKey(tag + version))
                        pre.TagId = Tags[tag + version];
                    else
                        throw new EdgarLineException(EdgarDatasetPresentation.FILE_NAME, lineNumber, "Error retrieving key: " + tag + version);
                }
                pre.PreferredLabelXBRLLinkRole = fields[fieldNames.IndexOf("prole")];
                pre.PreferredLabel = fields[fieldNames.IndexOf("plabel")];
                pre.Negating = !(fields[fieldNames.IndexOf("negating")] == "0");
                pre.LineNumber = lineNumber;

                string numKey = adsh + tag + version;
                if (Nums.ContainsKey(numKey))
                    pre.NumberId = Nums[numKey];
                else
                    pre.ADSH_Tag_Version = adsh + "|" + tag + "|" + version;

                if (Texts.ContainsKey(numKey))
                    pre.TextId = Texts[numKey];
                else
                    pre.ADSH_Tag_Version = adsh + "|" + tag + "|" + version;

                return pre;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public override IList<EdgarTuple> GetKeys(IEdgarDatasetsRepository repository, int datasetId)
        {
            return repository.GetPresentationsKeys(datasetId);
        }
    }
}
