using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalyst.Common.Exceptions.EdgarSEC
{
    public class InvalidFormatException:Exception
    {
        public InvalidFormatException(string message):base(message)
        {

        }
    }
}
