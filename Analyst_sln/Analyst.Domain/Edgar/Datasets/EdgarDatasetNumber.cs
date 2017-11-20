using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Domain.Edgar.Datasets
{
    /// <summary>
    /// The NUM data set contains numeric data, 
    /// one row per data point in the financial statements. 
    /// The source for the table is the "as filed" XBRL filer submissions.
    /// </summary>
    public class EdgarDatasetNumber:IEdgarDatasetFile
    {
        public const string FILE_NAME = "num.tsv";

        [Key]
        public int Id { get; set; }



        
        /// <summary>
        /// The end date for the data value, rounded to the nearest month end.
        /// DATE (yyyymmdd)
        /// </summary>
        //[Key]
        [Required]
        public DateTime DDate { get; set; }

        /// <summary>
        /// QTRS
        /// The count of the number of quarters represented by the data value, 
        /// rounded to the nearest whole number. 
        /// "0" indicates it is a point-in-time value.
        /// NUMERIC 8
        /// </summary>
        //[Key]
        [Required]
        public int CountOfNumberOfQuarters { get; set; }


        /// <summary>
        /// UOM
        /// The unit of measure for the value.
        /// ALPHANUMERIC 20
        /// </summary>
        //[Key]
        [Required]
        [StringLength(20)]
        public string UnitOfMeasure { get; set; }

       

        /// <summary>
        /// A positive integer to distinguish different reported facts 
        /// that otherwise would have the same primary key.  
        /// For most purposes, data with iprx greater than 1 are not needed.  
        /// The priority for the fact based on higher precision, 
        /// closeness of the end date to a month end, 
        /// and closeness of the duration to a multiple of three months. 
        /// See fields dcml, durp and datp.
        /// </summary>
        //[Key]
        [Required]
        public short IPRX { get; set; }


        /// <summary>
        /// The value. This is not scaled, it is as found in the Interactive Data file, but is rounded to four digits to the right of the decimal point.
        /// NUMERIC 16
        /// </summary>
        public double? Value { get; set; }

        /// <summary>
        /// The plain text of any superscripted footnotes on the value, if any, as shown on the statement page, truncated to 512 characters.
        /// </summary>
        [StringLength(512)]
        public string FootNote { get; set; }

        /// <summary>
        /// Number of bytes in the plain text of the footnote prior to truncation; zero if no footnote.
        /// </summary>
        [Required]
        public short FootLength { get; set; }

        /// <summary>
        /// Small integer representing the number of dimensions.  
        /// Note that this value is a function of the dimension segments.
        /// </summary>
        [Required]
        public short NumberOfDimensions { get; set; }


        /// <summary>
        /// If specified, indicates a specific co-registrant, the parent company, or other entity (e.g., guarantor).  
        /// NULL indicates the consolidated entity.  
        /// Note that this value is a function of the dimension segments.
        /// </summary>
        [StringLength(256)]
        public string CoRegistrant { get; set; }

        /// <summary>
        /// The difference between the reported fact duration and the quarter duration (qtrs), expressed as a fraction of 1.  
        /// For example, a fact with duration of 120 days rounded to a 91-day quarter has a durp value of 29/91 = +0.3187.
        /// </summary>
        [Required]
        public float durp { get; set; }


        /// <summary>
        /// The difference between the reported fact date and the month-end rounded date (ddate), expressed as a fraction of 1. 
        /// For example, a fact reported for 29/Dec, with ddate rounded to 31/Dec, has a datp value of minus 2/31 = -0.0645.
        /// </summary>
        [Required]
        public float datp { get; set; }


        /// <summary>
        /// The value of the fact "decimals" attribute, with INF represented by 32767.
        /// </summary>
        [Required]
        public int Decimals { get; set; }

        #region Not mapped fields
        /// <summary>
        /// FK to submission in the file
        /// </summary>
        [NotMapped]
        public string ADSH { get; set; }

        /// <summary>
        /// Compound FK (the other is tagstr) to tag in the file
        /// </summary>
        [NotMapped]
        public string TagStr { get; set; }


        /// <summary>
        /// Compound FK (the other is tagstr) to tag in the file
        /// </summary>
        [NotMapped]
        public string Version { get; set; }

        [NotMapped]
        public string TagCompoundKey { get { return this.TagStr + this.Version; } }

        [NotMapped]
        public string DimensionStr { get; set; }

        #endregion

        [Required]
        public int LineNumber { get; set; }

        public string Key
        {
            get
            {
                return Submission.ADSH + Tag.Tag + Tag.Version;
            }
        }

        [Required]
        public EdgarDatasetSubmission Submission { get; set; }

        [Required]
        public EdgarDatasetTag Tag { get; set; }

        [Required]
        public EdgarDatasetDimension Dimension { get; set; }

        public virtual ICollection<EdgarDatasetPresentation> Presentations { get; set; }

        
    }
}
