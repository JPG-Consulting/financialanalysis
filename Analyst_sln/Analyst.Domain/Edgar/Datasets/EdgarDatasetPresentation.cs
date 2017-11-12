﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Domain.Edgar.Datasets
{
    public class EdgarDatasetPresentation : IEdgarDatasetFile
    {
        public static readonly string FILE_NAME = "pre.tsv";

        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Represents the report grouping. 
        /// The numeric value refers to the "R file" as computed by the renderer and posted on the EDGAR website.  
        /// Note that in some situations the numbers skip.
        /// </summary>
        [Required]
        [Index("IX_ReportLine", 1, IsUnique = true)]
        public int ReportNumber { get; set; }

        public EdgarDatasetRendering Report { get; set; }

        /// <summary>
        /// Represents the tag's presentation line order for a given report. 
        /// Together with the statement and report field, presentation location, order and grouping can be derived.
        /// </summary>
        [Required]
        [Index("IX_ReportLine", 2, IsUnique = true)]
        public int Line { get; set; }

        [Required]
        public EdgarDatasetSubmission Submission { get; set; }

        public string Key
        {
            get
            {
                return Submission.ADSH + Report.ToString() + Line.ToString();
            }
        }

        /// <summary>
        /// The financial statement location to which the value of the "report" field pertains.
        /// (CP = Cover Page, BS = Balance Sheet, IS = Income Statement, CF = Cash Flow, EQ = Equity, CI = Comprehensive Income, UN = Unclassifiable Statement).
        /// </summary>
        [StringLength(2)]
        [Required]
        public string FinancialStatement{ get; set; }

        /// <summary>
        /// 1 indicates that the value was presented "parenthetically" instead of in fields within the financial statements.For example:
        /// Receivables(net of allowance for bad debts of $200 in 2012) $700.
        /// </summary>
        public bool Inpth { get; set; }

        [Required]
        public EdgarDatasetTag Tag { get; set; }

        /// <summary>
        /// The XBRL link "role" of the preferred label, using only the portion of the role URI after the last "/".
        /// </summary>
        [Required]
        [StringLength(50)]
        public string prole{ get; set; }

        /// <summary>
        /// The text presented on the line item, also known as a "preferred" label.
        /// </summary>
        [Required]
        [StringLength(512)]
        public string PreferredLabel { get; set; }


        /// <summary>
        /// Flag to indicate whether the prole is treated as negating by the renderer.
        /// </summary>
        [Required]
        public bool Negating { get; set; }
        public char RenderFile { get; set; }

        [Required]
        public EdgarDataset Dataset { get; set; }
        public int LineNumber { get; set; }
    }
}
