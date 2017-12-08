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
    /// <summary>
    /// Standard Industrial Classification (SIC). 
    /// Four digit code assigned by the Commission as of the filing date, 
    /// indicating the registrant's type of business.
    /// </summary>
    public class SIC:IEdgarEntity
    {
        [Key]
        public int Id { get; set; }

        [Index(IsUnique =true)]
        public short Code { get; set; }

        [StringLength(5)]
        public string ADOffice { get; set; }

        [StringLength(100)]
        public string IndustryTitle { get; set; }

        public string Key
        {
            get
            {
                return Code.ToString();
            }
        }
    }
}
