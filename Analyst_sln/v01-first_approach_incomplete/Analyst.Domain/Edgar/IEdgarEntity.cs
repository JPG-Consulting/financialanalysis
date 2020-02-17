using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Domain.Edgar
{
    public interface IEdgarEntity
    {
        int Id { get; set; }
        string Key { get; }
    }
}
