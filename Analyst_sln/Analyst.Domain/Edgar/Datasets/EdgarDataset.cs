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
    /// https://www.sec.gov/dera/data/financial-statement-and-notes-data-set.html
    /// </summary>
    [Serializable]
    public class EdgarDataset:IEdgarDatasetFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        public string RelativePath { get; set; }
        
        [Required]
        public int Year { get; set; }

        [Required]
        public Quarter Quarter { get; set; }

        public virtual IList<EdgarDatasetTag> Tags { get; set; }

        //[ForeignKey("EdgarDataset_Id")]
        public virtual IList<EdgarDatasetSubmissions> Submissions { get; set; }

        public virtual IList<EdgarDatasetDimension> Dimensions { get; set; }

        public virtual IList<EdgarDatasetNumber> Numbers { get; set; }

        public int TotalSubmissions { get; set; }

        public int ProcessedSubmissions { get; set; }

        public int TotalTags { get; set; }

        public int ProcessedTags { get; set; }

        public int TotalNumbers { get; set; }
        public int ProcessedNumbers { get; set; }
        

        public int ProcessedDimensions { get; set; }
        public int TotalDimensions { get; set; }

        public string Key
        {
            get
            {
                return Id.ToString();
            }
        }
    }
}
