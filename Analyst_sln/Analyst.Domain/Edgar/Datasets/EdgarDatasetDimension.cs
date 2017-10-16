using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Domain.Edgar.Datasets
{
    public class EdgarDatasetDimension
    {
        [Key]
        public int Id { get; set; }
    }
}
