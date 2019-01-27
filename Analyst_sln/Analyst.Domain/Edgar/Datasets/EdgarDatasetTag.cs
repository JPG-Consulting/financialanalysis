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
    /// TAG is a data set of all tags used in the submissions, both standard and custom.
    /// The TAG data set contains all standard taxonomy tags, 
    /// not just those appearing in submissions to date, 
    /// and also includes all custom taxonomy tags defined in the submissions.  
    /// The source is the "as filed" XBRL filer submissions.  
    /// The standard tags are derived from taxonomies in 
    /// https://www.sec.gov/info/edgar/edgartaxonomies.shtml 
    /// as of the date of the original submission. 
    /// --------------------------------------------------------
    /// Nota: esta es una tabla maestra
    /// Cada Tag representaría una cuenta contable
    /// Listando los tags correctos en el orden correcto, 
    /// se podría armar los estados contables
    /// </summary>
    public class EdgarDatasetTag: IEdgarDatasetFile
    {

        public const string FILE_NAME = "tag.tsv";

        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Tag used by the filer
        /// </summary>
        [StringLength(256)]
        [Required]
        public string Tag { get; set; }

        /// <summary>
        /// If a standard tag, the taxonomy of origin, otherwise equal to adsh.
        /// </summary>
        [StringLength(20)]
        [Required]
        public string Version { get; set; }

        /// <summary>
        /// 1 if tag is custom (version=adsh), 
        /// 0 if it is standard. 
        /// Note: This flag is technically redundant with the version and adsh fields.
        /// </summary>
        public bool Custom { get; set; }

        /// <summary>
        /// 1 if the tag is not used to represent a numeric fact.
        /// </summary>
        public bool Abstract{ get; set; }

        /// <summary>
        /// If abstract=1, then NULL, 
        /// otherwise the data type (e.g., monetary) for the tag.
        /// Field name in the file: datatype
        /// </summary>
        [StringLength(25)]
        public string Datatype { get; set; }

        /// <summary>
        /// If abstract=1, then NULL; 
        /// otherwise, "I" if the value is a point in time, or "D" if the value is a duration.
        /// Field name in the file: Iord
        /// </summary>
        [StringLength(1)]
        public string ValueTypeStr { get; set; }
        public char? ValueType
        {
            get
            {
                if (string.IsNullOrEmpty(ValueTypeStr))
                    return null;
                if (ValueTypeStr.Length != 1)
                    return null;
                return ValueTypeStr[0];
            }

            set { ValueTypeStr = value.ToString(); }
        }
        
        

        /// <summary>
        /// Another getter of ValueType
        /// </summary>
        /// <returns></returns>
        public ValueType ValueTypeAsEnum()
        {
            if (!this.ValueType.HasValue)
                return ValueTypeValue.NoValue;
            if (ValueType.Value.Equals("D"))
                return ValueTypeValue.Duration;
            return ValueTypeValue.PointInTime;
        }

        /// <summary>
        /// Possible values for the ValueType property.
        /// </summary>
        public enum ValueTypeValue
        {
            NoValue,
            PointInTime,
            Duration
        }


        /// <summary>
        /// If datatype = monetary, then the tag's natural accounting balance 
        /// from the perspective of the balance sheet 
        /// or income statement (debit or credit); 
        /// if not defined, then NULL.
        /// Field name in the file: crdr
        /// </summary>
        [StringLength(1)]
        public string NaturalAccountingBalanceStr { get; set; }
        public char? NaturalAccountingBalance
        {
            get
            {
                if (string.IsNullOrEmpty(NaturalAccountingBalanceStr))
                    return null;
                if (NaturalAccountingBalanceStr.Length != 1)
                    return null;
                return NaturalAccountingBalanceStr[0];
            }

            set { NaturalAccountingBalanceStr = value.ToString(); }
        }

        /// <summary>
        /// Another getter of NaturalAccountingBalance
        /// </summary>
        public NaturalAccountingBalanceValue NaturalAccountingBalanceAsEnum()
        {
            if (!NaturalAccountingBalance.HasValue)
                return NaturalAccountingBalanceValue.NoValue;
            if (NaturalAccountingBalance.Value.Equals("D") == true)
                return NaturalAccountingBalanceValue.Debit;
            return NaturalAccountingBalanceValue.Credit;
        }

        /// <summary>
        /// Possible values for the NaturalAccountingBalance property.
        /// </summary>
        public enum NaturalAccountingBalanceValue
        {
            NoValue,
            Credit,
            Debit
        }

        /// <summary>
        /// Tag label
        /// If a standard tag, then the label text provided by the taxonomy, 
        /// otherwise the text provided by the filer.  
        /// A tag which had neither would have a NULL value here.
        /// Field name in the file: tlabel
        /// </summary>
        [StringLength(512)]
        public string LabelText { get; set; }

        /// <summary>
        /// The detailed definition for the tag, truncated to 2048 characters. 
        /// If a standard tag, then the text provided by the taxonomy, 
        /// otherwise the text assigned by the filer.  
        /// Some tags have neither, in which case this field is NULL.
        /// </summary>
        [StringLength(2048)]
        public string Documentation { get; set; }

        public int LineNumber { get; set; }

        /// <summary>
        /// Compund Key = ADSH and Version
        /// </summary>
        [NotMapped]
        public string Key
        {
            get
            {
                return Tag + Version;
            }

        }

        public int DatasetId { get; set; }

        public EdgarDataset Dataset { get; set; }


        public virtual ICollection<EdgarDatasetNumber> Numbers { get; set; }

        public virtual ICollection<EdgarDatasetText> Texts { get; set; }

        public virtual ICollection<EdgarDatasetPresentation> Presentations { get; set; }

        public virtual ICollection<EdgarDatasetCalculation> ParentCalculations { get; set; }

        public virtual ICollection<EdgarDatasetCalculation> ChildCalculations { get; set; }

    }
}
