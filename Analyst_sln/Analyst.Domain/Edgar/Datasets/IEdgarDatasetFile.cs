using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Domain.Edgar.Datasets
{
    public interface IEdgarDatasetFile:IEdgarEntity
    {
        int LineNumber { get; set; }
        int DatasetId { get; }
    }
}
