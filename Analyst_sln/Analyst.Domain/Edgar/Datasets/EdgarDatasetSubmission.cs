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
    /// It identifies all the EDGAR submissions in the data set, 
    /// with each row having the unique (primary) key adsh.
    /// The submissions data set contains summary information about an entire EDGAR submission
    /// 
    /// Note: To access the complete submission files for a given filing, please see the Commission EDGAR website.  
    /// The Commission website folder 
    /// https://www.sec.gov/Archives/edgar/data/{cik}/{accession}/
    /// will always contain all the data sets for a given submission. 
    /// To assemble the folder address to any filing referenced in the SUB data set, 
    /// simply substitute {cik} with the cik field 
    /// and replace {accession} with the adsh field (after removing the dash character).  
    /// SQL Query Sample:
    /// select 
    ///     name,form,period, 
    ///     'https://www.sec.gov/Archives/edgar/data/' + ltrim(str(cik,10))+'/' + replace(adsh,'-','')+'/'+instance as url 
    /// from sub 
    /// order by period desc, name
    /// ---------------------------------------------
    /// Representa cada documento (form) 
    /// subido por las registrants (compañias, mutual funds, etc)
    /// </summary>
    [Serializable]
    public class EdgarDatasetSubmission:IEdgarDatasetFile
    {
        public const string FILE_NAME = "sub.tsv";

        [Key]
        public int Id { get; set; }


        /// <summary>
        /// Accession Number:
        /// it's the Unique Key of each submission.
        /// The 20-character string formed 
        /// from the 18-digit number assigned by the Commission to each EDGAR submission.
        /// </summary>
        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        //[Index(IsUnique = true)]Due the collate of this column (it has to be case sensitive), index has to be created after change collation
        [Required]
        public string ADSH { get; set; }

        [Required]
        public Registrant Registrant { get; set; }

        [Required]
        public SECForm Form { get; set; }

        /// <summary>
        /// Balance Sheet Date.
        /// </summary>
        [Required]
        public DateTime Period { get; set; }

        /// <summary>
        /// TRUE indicates that the XBRL submission contains quantitative disclosures within the footnotes and schedules at the required detail level (e.g., each amount).
        /// </summary>
        [Required]
        public bool Detail { get; set; }

        

        /// <summary>
        /// The name of the submitted XBRL Instance Document (EX-101.INS) type data file. 
        /// The name often begins with the company ticker symbol.
        /// Field name in the file: instance
        /// </summary>
        [Required]
        [StringLength(32)]
        public string XBRLInstance { get; set; }

        /// <summary>
        /// nciks
        /// Number of Central Index Keys (CIK) of registrants (i.e., business units) included in the consolidating entity's submitted filing.
        /// </summary>
        [Required]
        public int NumberOfCIKs { get; set; }

        /// <summary>
        /// aciks
        /// Additional CIKs of co-registrants included in a consolidating entity's EDGAR submission, separated by spaces. 
        /// If there are no other co-registrants (i.e., nciks = 1), the value of aciks is NULL.  
        /// For a very small number of filers, the list of co-registrants is too long to fit in the field.  
        /// Where this is the case, PARTIAL will appear at the end of the list indicating that not all co-registrants' CIKs are included in the field; 
        /// users should refer to the complete submission file for all CIK information.
        /// Field name in the file: aciks
        /// </summary>
        [StringLength(120)]
        public string AdditionalCIKs { get; set; }

        /// <summary>
        /// Public float, in USD, if provided in this submission.
        /// </summary>
        public float? PublicFloatUSD { get; set; }

        /// <summary>
        /// Date on which the public float was measured by the filer.
        /// </summary>
        public DateTime? FloatDate { get; set; }


        /// <summary>
        /// If the public float value was computed by summing across several tagged values, this indicates the nature of the summation.
        /// </summary>
        [StringLength(255)]
        public String FloatAxis { get; set; }

        /// <summary>
        /// If the public float was computed, the number of terms in the summation.
        /// </summary>
        public int? FloatMems { get; set; }

        public int LineNumber { get; set; }

        public string Key
        {
            get
            {
                return ADSH;
            }
        }

        public int DatasetId { get; set; }
        
        [Required]
        public EdgarDataset Dataset { get; set; }

        public virtual ICollection<EdgarDatasetRender> Renders { get; set; }

        public virtual ICollection<EdgarDatasetNumber> Numbers { get; set; }

        public virtual ICollection<EdgarDatasetText> Texts { get; set; } 

        public virtual ICollection<EdgarDatasetPresentation> Presentations { get; set; }

        public virtual ICollection<EdgarDatasetCalculation> Calculations { get; set; }
        public int RegistrantId { get; set; }
        public int SECFormId { get; set; }
    }
}
