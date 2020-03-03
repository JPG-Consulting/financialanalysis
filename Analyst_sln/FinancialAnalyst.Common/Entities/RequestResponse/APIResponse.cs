using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.RequestResponse
{
    public class APIResponse<T>
    {
        public T Content { get; set; }
        public bool Ok { get; set; }
        public string ErrorMessage { get; set; }
    }
}
