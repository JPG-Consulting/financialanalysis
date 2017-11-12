using Analyst.Domain;
using Analyst.Domain.Edgar;
using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.DBAccess.Contexts
{
    public class AnalystContext : DbContext
    {

        static AnalystContext()
        {
            Database.SetInitializer<AnalystContext>(new AnalystContextInitializer());
        }

        public AnalystContext() : base("name=Analyst")
        {
        }



        public virtual DbSet<EdgarDataset> DataSets { get; set; }

        public virtual DbSet<SECForm> SECForms { get; set; }

        public virtual DbSet<SIC> SICs { get; set; }

        public virtual DbSet<Registrant> Registrants { get; set; }

        public virtual DbSet<EdgarDatasetSubmission> Submissions { get; set; }
        public virtual DbSet<EdgarDatasetTag> Tags { get; set; }

        public virtual DbSet<EdgarDatasetNumber> Numbers { get; set; }
        public virtual DbSet<EdgarDatasetDimension> Dimensions { get; set; }

        public virtual DbSet<EdgarDatasetRendering> Renders { get; set; }
    
        public virtual DbSet<EdgarDatasetPresentation> Presentations { get; set; }
    }
    

}
