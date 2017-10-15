using Analyst.DBAccess;
using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Services
{
    public class EdgarService
    {
        public AnalystContext Context { get; set; }

        public List<EdgarDataset> GetDatasets()
        {
            Context = new AnalystContext();
            return Context.GetDatasets();
        }
    }
}
