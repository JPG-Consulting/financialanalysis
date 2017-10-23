using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Domain.Edgar.Datasets
{
    /// <summary>
    /// https://www.sec.gov/dera/data/financial-statement-and-notes-data-set.html
    /// </summary>
    [Serializable]
    public class EdgarDataset
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string RelativePath { get; set; }
        
        [Required]
        public int Year { get; set; }

        [Required]
        public Quarter Quarter { get; set; }

        public virtual IList<EdgarDatasetTag> Tags { get; set; }

        public virtual IList<EdgarDatasetSubmissions> Submissions { get; set; }

        public virtual IList<EdgarDatasetDimension> Dimensions { get; set; }

        public int TotalSubmissions { get; set; }

        public int ProcessedSubmissions
        {
            get
            {
                if (Submissions != null)
                    return Submissions.Count;
                else
                    return 0;
            }
        }

        public int TagsProcessed { get; set; }
        public int SubmissionsProcessed { get; set; }
    }
}
