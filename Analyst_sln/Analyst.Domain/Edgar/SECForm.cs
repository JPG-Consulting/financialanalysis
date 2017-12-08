using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Domain.Edgar
{
    [Serializable]
    public class SECForm: IEdgarEntity
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [StringLength(20)]
        [Index(IsUnique =true)]
        public String Code { get; set; }

        [StringLength(300)]
        public string Description { get; set; }

        [StringLength(100)]
        public string LinkToPdf { get; set; }

        [StringLength(20)]
        public string LastUpddate { get; set; }

        [StringLength(20)]
        public string SECNumber { get; set; }

        [StringLength(100)]
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
