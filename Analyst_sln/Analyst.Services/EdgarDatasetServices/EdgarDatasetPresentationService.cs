﻿using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyst.DBAccess.Contexts;
using System.Collections.Concurrent;

namespace Analyst.Services.EdgarDatasetServices
{
    public interface IEdgarDatasetPresentationService : IEdgarFileService<EdgarDatasetPresentation>
    {
        ConcurrentDictionary<string, EdgarDatasetRendering> Renders { get; set; }
        ConcurrentDictionary<string, EdgarDatasetSubmission> Subs { get; set; }
        ConcurrentDictionary<string, EdgarDatasetTag> Tags { get; set; }
    }
    public class EdgarDatasetPresentationService : EdgarFileService<EdgarDatasetPresentation>, IEdgarDatasetPresentationService
    {
        public ConcurrentDictionary<string, EdgarDatasetRendering> Renders { get; set; }
        public ConcurrentDictionary<string,EdgarDatasetSubmission> Subs { get; set; }
        public ConcurrentDictionary<string, EdgarDatasetTag> Tags { get; set; }

        public override void Add(IAnalystRepository repo, EdgarDataset dataset, EdgarDatasetPresentation file)
        {
            repo.Add(dataset,file);
        }

        public override EdgarDatasetPresentation Parse(IAnalystRepository repository, List<string> fieldNames, List<string> fields, int lineNumber)
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
                pre.Submission = Subs[adsh];
                string report = fields[fieldNames.IndexOf("report")];
                pre.Report = Renders[adsh + report];
                pre.Line = Convert.ToInt32(fields[fieldNames.IndexOf("line")]);
                pre.FinancialStatement = fields[fieldNames.IndexOf("stmt")];
                pre.Inpth = fields[fieldNames.IndexOf("inpth")] == "1";
                pre.RenderFile = fields[fieldNames.IndexOf("rfile")][0];
                string tag = fields[fieldNames.IndexOf("tag")];
                string version = fields[fieldNames.IndexOf("version")];
                pre.Tag = Tags[tag + version];
                pre.prole = fields[fieldNames.IndexOf("prole")];
                pre.PreferredLabel = fields[fieldNames.IndexOf("plabel")];
                pre.Negating = !(fields[fieldNames.IndexOf("negating")] == "0");
                pre.LineNumber = lineNumber;
                return pre;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
