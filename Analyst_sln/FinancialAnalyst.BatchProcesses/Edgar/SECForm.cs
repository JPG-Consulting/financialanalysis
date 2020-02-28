using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Domain.Edgar
{
    [Serializable]
    [DataContract]
    public class SECForm : IEdgarEntity
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [StringLength(20)]
        [Index(IsUnique = true)]
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
