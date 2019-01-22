using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Domain.Edgar.Indexes
{
    [Serializable]
    [DataContract]
    public abstract class IndexBase<T> : IEdgarEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public abstract string Key { get; }

        [Required]
        [DataMember]
        public int Year { get; set; }

        [Required]
        [DataMember]
        public Quarter Quarter { get; set; }

        [NotMapped]
        public string RelativeURL { get; set; }


        /// <summary>
        /// It indicates if all the forms of this index are stored (and ready to query) or not
        /// </summary>
        [DataMember]
        public bool IsComplete { get; set; }

        public IList<IndexEntry> Entries { get; set; }
        
    }
}
