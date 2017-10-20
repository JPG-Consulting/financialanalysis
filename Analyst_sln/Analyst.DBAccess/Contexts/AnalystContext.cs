using Analyst.Domain;
using Analyst.Domain.Edgar;
using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.DBAccess.Contexts
{
    internal class AnalystContext : DbContext
    {

        public AnalystContext()
            : base("name=Analyst")
        {
        }

        public virtual DbSet<EdgarDataset> DataSets { get; set; }

        public virtual DbSet<SECForm> SECForms { get; set; }

        public virtual DbSet<SIC> SICs { get; set; }

        public virtual DbSet<Registrant> Registrants { get; set; }

        public virtual DbSet<EdgarDatasetSubmissions> Submissions { get; set; }
        
    }
}
