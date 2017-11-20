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
            //http://www.entityframeworktutorial.net/EntityFramework4.3/lazy-loading-with-dbcontext.aspx
            //this.Configuration.LazyLoadingEnabled = true;
            //this.Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            //https://stackoverflow.com/questions/5559043/entity-framework-code-first-two-foreign-keys-from-same-table
            modelBuilder.Entity<EdgarDatasetCalculation>()
                .HasRequired(calc => calc.ParentTag)
                .WithMany(tag => tag.ParentCalculations)
                .HasForeignKey(calc => calc.ParentTagId)
                .WillCascadeOnDelete(false)
                ;

            modelBuilder.Entity<EdgarDatasetCalculation>()
                .HasRequired(calc => calc.ChildTag)
                .WithMany(tag => tag.ChildCalculations)
                .HasForeignKey(calc => calc.ChildTagId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EdgarDatasetPresentation>()
                .HasRequired(pre => pre.Render)
                .WithMany(ren => ren.Presentations)
                .HasForeignKey(pre => pre.RenderId)
                .WillCascadeOnDelete(false);
        }


        public virtual DbSet<EdgarDataset> DataSets { get; set; }

        public virtual DbSet<SECForm> SECForms { get; set; }

        public virtual DbSet<SIC> SICs { get; set; }

        public virtual DbSet<Registrant> Registrants { get; set; }

        public virtual DbSet<EdgarDatasetSubmission> Submissions { get; set; }
        public virtual DbSet<EdgarDatasetTag> Tags { get; set; }

        public virtual DbSet<EdgarDatasetNumber> Numbers { get; set; }
        public virtual DbSet<EdgarDatasetDimension> Dimensions { get; set; }

        public virtual DbSet<EdgarDatasetRender> Renders { get; set; }
    
        public virtual DbSet<EdgarDatasetPresentation> Presentations { get; set; }

        public virtual DbSet<EdgarDatasetCalculation> Calculations { get; set; }

        public virtual DbSet<EdgarDatasetText> Texts { get; set; }
    }
    

}
