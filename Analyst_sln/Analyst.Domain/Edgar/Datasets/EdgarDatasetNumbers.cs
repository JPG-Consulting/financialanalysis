using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Domain.Edgar.Datasets
{
    public class EdgarDatasetNumbers
    {
        [Key]
        public int Id { get; set; }

        public string ADSH { get; set; }
    }
}
