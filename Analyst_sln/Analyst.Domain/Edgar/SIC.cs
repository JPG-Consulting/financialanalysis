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
    public class SIC
    {
        [Key]
        public short Code { get; set; }

        public string IndustryTitle { get; set; }
    }
}
