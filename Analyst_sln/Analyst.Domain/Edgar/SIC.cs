using System;
using System.Collections.Generic;
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
        public short Id { get; set; }

        public string Name { get; set; }
    }
}
