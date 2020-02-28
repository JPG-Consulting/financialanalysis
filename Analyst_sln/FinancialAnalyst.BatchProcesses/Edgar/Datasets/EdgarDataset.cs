using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Domain.Edgar.Datasets
{
    /// <summary>
    /// https://www.sec.gov/dera/data/financial-statement-and-notes-data-set.html
    /// </summary>
    [Serializable]
    [DataContract]
    public class EdgarDataset : IEdgarEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DataMember]
        public int Id { get; set; }

        [Required]
        [DataMember]
        public string RelativePath { get; set; }

        [Required]
        [DataMember]
        public int Year { get; set; }

        [Required]
        [DataMember]
        public Quarter Quarter { get; set; }

        public virtual IList<EdgarDatasetCalculation> Calculations { get; set; }

        public virtual IList<EdgarDatasetDimension> Dimensions { get; set; }
        public virtual IList<EdgarDatasetNumber> Numbers { get; set; }
        public virtual IList<EdgarDatasetPresentation> Presentations { get; set; }

        public virtual IList<EdgarDatasetRender> Renders { get; set; }

        public virtual IList<EdgarDatasetSubmission> Submissions { get; set; }

        public virtual IList<EdgarDatasetTag> Tags { get; set; }
        public virtual IList<EdgarDatasetText> Text { get; set; }

        [DataMember]
        public int TotalSubmissions { get; set; }

        [DataMember]
        public int ProcessedSubmissions { get; set; }

        [DataMember]
        public int TotalTags { get; set; }

        [DataMember]
        public int ProcessedTags { get; set; }

        [DataMember]
        public int TotalNumbers { get; set; }

        [DataMember]
        public int ProcessedNumbers { get; set; }

        [DataMember]
        public int ProcessedDimensions { get; set; }

        [DataMember]
        public int TotalDimensions { get; set; }

        [DataMember]
        public int ProcessedRenders { get; set; }

        [DataMember]
        public int TotalRenders { get; set; }

        [DataMember]
        public int ProcessedPresentations { get; set; }

        [DataMember]
        public int TotalPresentations { get; set; }

        [DataMember]
        public int ProcessedCalculations { get; set; }

        [DataMember]
        public int TotalCalculations { get; set; }

        [DataMember]
        public int ProcessedTexts { get; set; }

        [DataMember]
        public int TotalTexts { get; set; }

        public string Key
        {
            get
            {
                return Id.ToString();
            }
        }

        [DataMember]
        public bool IsComplete
        {
            get
            {
                return TotalSubmissions == ProcessedSubmissions && TotalSubmissions > 0 &&
                        TotalTags == ProcessedTags && TotalTags > 0 &&
                        TotalNumbers == ProcessedNumbers && TotalNumbers > 0 &&
                        ProcessedDimensions == TotalDimensions && TotalDimensions > 0 &&
                        ProcessedRenders == TotalRenders && TotalRenders > 0 &&
                        ProcessedPresentations == TotalPresentations && TotalPresentations > 0 &&
                        ProcessedCalculations == TotalCalculations && TotalCalculations > 0 &&
                        ProcessedTexts == TotalTexts && TotalTexts > 0;
            }
        }

    }
}
