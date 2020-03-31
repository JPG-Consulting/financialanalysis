using FinancialAnalyst.Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FinancialAnalyst.Common.Entities.EdgarSEC.Indexes
{
    [Serializable]
    [DataContract]
    public class MasterIndex : IEdgarEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Key
        {
            get
            {
                if (IndexDate.HasValue)
                    return Year.ToString() + Quarter.ToString();
                else
                    return Year.ToString() + Quarter.ToString() + IndexDate.Value.ToString("yyyyMMdd");
            }
        }

        [Required]
        [DataMember]
        public int Year { get; set; }

        [Required]
        [DataMember]
        public Quarter Quarter { get; set; }

        /// <summary>
        /// If IndexDate is null, it's a quarter full index
        /// If IndexDate has a value, it's a daily index
        /// </summary>
#pragma warning disable CA2235 // Mark all non-serializable fields
        public DateTime? IndexDate { get; }
#pragma warning restore CA2235 // Mark all non-serializable fields


        /// <summary>
        /// It indicates if all the forms of this index are stored (and ready to query) or not
        /// </summary>
        [DataMember]
        public bool IsComplete { get; set; }

        [DataMember]
        public long TotalLines { get; set; }
        
        [DataMember]
        public long ProcessedLines { get; set; }

        public IList<IndexEntry> Entries { get; set; }
        
    }
}
