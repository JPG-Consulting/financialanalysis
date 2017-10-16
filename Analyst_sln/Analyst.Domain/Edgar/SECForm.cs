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
    public class SECForm
    {
        [Key]
        [Index(IsUnique =true)]
        public String Code { get; set; }

        public string Description { get; set; }

        public string LinkToPdf { get; set; }

        public string LastUpddate { get; set; }

        public string SECNumber { get; set; }

        public string Topic { get; set; }
    }
}
