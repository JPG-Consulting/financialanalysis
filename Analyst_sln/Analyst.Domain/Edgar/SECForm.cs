using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Domain.Edgar
{
    [Serializable]
    public class SECForm
    {
        public int Id { get; set; }

        public String Code { get; set; }

        public string Description { get; set; }
    }
}
