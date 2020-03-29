using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalyst.Common.Entities.EdgarSEC.Datasets
{
    public class EdgarDatasetCalculation : IEdgarDatasetFile
    {
        public static readonly string FILE_NAME = "cal.tsv";

        public int Id { get; set; }
        
        
        public int SubmissionId { get; set; }
        [Required]
        public EdgarDatasetSubmission Submission { get; set; }

        /// <summary>
        /// grp
        /// Sequential number for grouping arcs in a submission.
        /// </summary>
        [Required]
        public short SequentialNumberForGrouping { get; set; }

        /// <summary>
        /// arc
        /// Sequential number for arcs within a group in a submission.
        /// </summary>
        [Required]
        public short SequentialNumberForArc { get; set; }

        /// <summary>
        /// Indicates a weight of -1 (TRUE if the arc is negative), but typically +1 (FALSE).
        /// </summary>
        [Required]
        public bool Negative { get; set; }

        /// <summary>
        /// The tag for the parent of the arc
        /// The version of the tag for the parent of the arc
        /// </summary>
        [Required]
        public EdgarDatasetTag ParentTag { get; set; }

        /// <summary>
        /// The tag for the child of the arc
        /// The version of the tag for the child of the arc
        /// </summary>
        [Required]
        public EdgarDatasetTag ChildTag { get; set; }

        public int LineNumber { get; set; }

        public string Key
        {
            get
            {
                return Submission.ADSH + SequentialNumberForGrouping.ToString() + SequentialNumberForArc.ToString();
            }
        }

        public int DatasetId { get; set; }

        [Required]
        public EdgarDataset Dataset { get; set; }

        public int ParentTagId { get; set; }

        public int ChildTagId { get; set; }
    }
}
