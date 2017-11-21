using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Domain.Edgar.Datasets
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class CaseSensitiveAttribute : Attribute
    {
        public CaseSensitiveAttribute()
        {
            IsEnabled = true;
        }
        public bool IsEnabled { get; set; }
    }


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
        [Column(TypeName = "VARCHAR")]
        [StringLength(256)]
        [Required]
        public string Tag { get; set; }

        /// <summary>
        /// If a standard tag, the taxonomy of origin, otherwise equal to adsh.
        /// </summary>
        [Column(TypeName = "VARCHAR")]
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
        /// </summary>
        public string Datatype { get; set; }

        /// <summary>
        /// f abstract=1, then NULL; 
        /// otherwise, "I" if the value is a point in time, or "D" if the value is a duration.
        /// </summary>
        public char? Iord { get; set; }

        /// <summary>
        /// If datatype = monetary, then the tag's natural accounting balance 
        /// from the perspective of the balance sheet 
        /// or income statement (debit or credit); 
        /// if not defined, then NULL.
        /// </summary>
        public char? Crdr { get; set; }


        /// <summary>
        /// Tag label
        /// If a standard tag, then the label text provided by the taxonomy, 
        /// otherwise the text provided by the filer.  
        /// A tag which had neither would have a NULL value here.
        /// </summary>
        public string Tlabel { get; set; }

        /// <summary>
        /// The detailed definition for the tag, truncated to 2048 characters. 
        /// If a standard tag, then the text provided by the taxonomy, 
        /// otherwise the text assigned by the filer.  
        /// Some tags have neither, in which case this field is NULL.
        /// </summary>
        public string Doc { get; set; }

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

        public virtual IList<EdgarDatasetCalculation> ParentCalculations { get; set; }

        public virtual IList<EdgarDatasetCalculation> ChildCalculations { get; set; }

    }
}
