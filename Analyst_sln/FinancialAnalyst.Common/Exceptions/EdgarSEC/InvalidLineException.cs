using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalyst.Common.Exceptions.EdgarSEC
{
    public class InvalidLineException : Exception
    {
        public InvalidLineException(string message) : base(message)
        {
        }
    }
}
