using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Domain.Edgar.Datasets
{
    /// <summary>
    /// The TXT data set contains non-numeric data, one row per data point in the financial statements. The source for the table is the "as filed" XBRL filer submissions.
    /// </summary>
    public class EdgarDatasetText : IEdgarDatasetFile
    {
        public static readonly string FILE_NAME = "txt.tsv";

        public int Id { get; set; }

        public string Key
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int LineNumber { get; set; }

        [Required]
        public EdgarDatasetSubmission Submission { get; set; }

        [Required]
        public EdgarDatasetTag Tag { get; set; }
        /// <summary>
        /// The end date for the data value, rounded to the nearest month end.
        /// </summary>
        [Required]
        public DateTime DDate { get; set; }
        /// <summary>
        /// The count of the number of quarters represented by the data value, rounded to the nearest whole number. A point in time value is represented by 0.
        /// </summary>
        [Required]
        public int Qtrs { get; set; }

        /// <summary>
        /// A positive integer to distinguish different reported facts that otherwise would have the same primary key.  For most purposes, data with iprx greater than 1 are not needed.  The priority for the fact based on higher precision, closeness of the end date to a month end, and closeness of the duration to a multiple of three months. See fields dcml, durp and datp below.
        /// </summary>
        [Required]
        public short Iprx { get; set; }

        /// <summary>
        /// The ISO language code of the fact content.
        /// </summary>
        [StringLength(5)]
        [Required]
        public string Language { get; set; }

        /// <summary>
        /// The value of the fact "xml:lang" attribute, en-US represented by 32767, other "en" dialects having lower values, and other languages lower still.
        /// </summary>
        [Required]
        public int Dcml { get; set; }

        /// <summary>
        /// The difference between the reported fact duration and the quarter duration (qtrs), expressed as a fraction of 1.  For example, a fact with duration of 120 days rounded to a 91-day quarter has a durp value of 29/91 = +0.3187.
        /// </summary>
        [Required]
        public float Durp { get; set; }

        /// <summary>
        /// The difference between the reported fact date and the month-end rounded date (ddate), expressed as a fraction of 1.  For example, a fact reported for 29/Dec, with ddate rounded to 31/Dec, has a datp value of minus 2/31 = -0.0645.
        /// </summary>
        [Required]
        public float Datp { get; set; }

        [Required]
        public EdgarDatasetDimension Dimension { get; set; }

        /// <summary>
        /// Small integer representing the number of dimensions, useful for sorting.Note that this value is function of the dimension segments.
        /// </summary>
        public short? DimN { get; set; }

        /// <summary>
        /// If specified, indicates a specific co-registrant, the parent company, or other entity (e.g., guarantor).  NULL indicates the consolidated entity.  Note that this value is a function of the dimension segments.
        /// </summary>
        public int? Coreg { get; set; }

        /// <summary>
        /// Flag indicating whether the value has had tags removed.
        /// </summary>
        [Required]
        public bool Escaped { get; set; }

        /// <summary>
        /// Number of bytes in the original, unprocessed value.  Zero indicates a NULL value.
        /// </summary>
        [Required]
        public int SrcLen { get; set; }

        /// <summary>
        /// The original length of the whitespace normalized value, which may have been greater than 8192.
        /// </summary>
        public int TxtLen { get; set; }

        /// <summary>
        /// The plain text of any superscripted footnotes on the value, as shown on the page, truncated to 512 characters, or if there is no footnote, then this field will be blank.
        /// </summary>
        [StringLength(512)]
        public string FootNote { get; set; }

        /// <summary>
        /// Number of bytes in the plain text of the footnote prior to truncation.
        /// </summary>
        public int? FootLen { get; set; }


        /// <summary>
        /// The value of the contextRef attribute in the source XBRL document, which can be used to recover the original HTML tagging if desired.
        /// </summary>
        [StringLength(255)]
        [Required]
        public string Context { get; set; }

        /// <summary>
        /// The value, with all whitespace normalized, that is, all sequences of line feeds, carriage returns, tabs, non-breaking spaces, and spaces having been collapsed to a single space, and no leading or trailing spaces.  Escaped XML that appears in EDGAR "Text Block" tags is processed to remove all mark-up (comments, processing instructions, elements, attributes).  The value is truncated to a maximum number of bytes.  The resulting text is not intended for end user display but only for text analysis applications. 
        /// </summary>
        [StringLength(2048)]
        public string Value { get; set; }

        [Required]
        public EdgarDataset Dataset { get; set; }
    }
}
