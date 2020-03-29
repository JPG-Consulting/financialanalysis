using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FinancialAnalyst.Common.Entities.EdgarSEC
{
    [Serializable]
    [DataContract]
    public class SECForm : IEdgarEntity
    {
        [Key]
        public int Id { get; set; }

        // https://docs.microsoft.com/es-es/ef/core/modeling/indexes
        // https://github.com/jsakamoto/EntityFrameworkCore.IndexAttribute
        //[Index(IsUnique = true)]
        [Column(TypeName = "NVARCHAR")]
        [StringLength(20)]
        [DataMember]
        public String Code { get; set; }

        [StringLength(300)]
        [DataMember]
        public string Description { get; set; }

        [StringLength(100)]
        [DataMember]
        public string LinkToPdf { get; set; }

        [StringLength(20)]
        [DataMember]
        public string LastUpddate { get; set; }

        [StringLength(20)]
        [DataMember]
        public string SECNumber { get; set; }

        [StringLength(100)]
        [DataMember]
        public string Topic { get; set; }

        public string Key
        {
            get
            {
                return Code;
            }
        }
    }
}
