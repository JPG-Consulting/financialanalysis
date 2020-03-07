using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Domain.Edgar.Datasets
{
    public class EdgarDatasetDimension:IEdgarDatasetFile
    {
        public static readonly int LENGHT_FIELD_DIMENSIONH = 34;
        public const string FILE_NAME = "dim.tsv";

        [Key]
        public int Id { get; set; }

        /// <summary>
        /// MD5 hash of the segments field text.  
        /// Although MD5 is unsuitable for cryptographic use, 
        /// it is used here merely to limit the size of the primary key.
        /// </summary>
        // https://docs.microsoft.com/es-es/ef/core/modeling/indexes
        // https://github.com/jsakamoto/EntityFrameworkCore.IndexAttribute
        //[Index("IX_DimensionH", IsUnique = true,Order =1)]
        [StringLength(34)]//32 hexa + "0x"
        public string DimensionH { get; set; }

        /// <summary>
        /// Concatenation of tag names representing the axis and members appearing in the XBRL segments.
        /// Tag names have their first characters "Statement", last 4 characters "Axis", and last 6 characters "Member" or "Domain" truncated where they appear.
        /// Namespaces and prefixes are ignored because EDGAR validation guarantees that the local-names are unique with a submission. 
        /// Each dimension is represented as the pair "{axis}={member};" and the axes concatenated in lexical order. 
        /// Example: "LegalEntity=Xyz;Scenario=Restated;" represents the XBRL segment with dimension LegalEntityAxis and member XyzMember, dimension StatementScenarioAxis and member RestatedMember.
        /// </summary>
        [StringLength(1024)]
        public string Segments { get; set; }

        /// <summary>
        /// TRUE if the segments field would have been longer than 1024 characters had it not been truncated, else FALSE.
        /// </summary>
        public bool SegmentTruncated { get; set; }

        

        public int LineNumber { get; set; }

        public string Key
        {
            get
            {
                return DimensionH;
            }
        }

        // https://docs.microsoft.com/es-es/ef/core/modeling/indexes
        // https://github.com/jsakamoto/EntityFrameworkCore.IndexAttribute
        //[Index("IX_DimensionH", IsUnique = true, Order = 2)]
        public int DatasetId { get; set; }

        public virtual EdgarDataset Dataset { get; set; } 

        public virtual ICollection<EdgarDatasetNumber> Numbers { get; set; }

        public virtual ICollection<EdgarDatasetText> Texts { get; set; }


    }
}
