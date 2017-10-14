using System;
using System.Collections.Generic;
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
        public int Id { get; set; }
        public string RelativePath { get; set; }
        
        public int Year { get; set; }        

        public Quarter Quarter { get; set; }

        public List<EdgarDatasetTag> Tags { get; set; }

        public List<EdgarDatasetSubmissions> Submissions { get; set; }

        public List<EdgarDatasetDimension> Dimensions { get; set; }

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


    }
}
