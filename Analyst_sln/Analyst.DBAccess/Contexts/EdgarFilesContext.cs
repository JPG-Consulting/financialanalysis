using Analyst.Domain.Edgar;
using Analyst.Domain.Edgar.Indexes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.DBAccess.Contexts
{
    internal class EdgarFilesContext : DbContext
    {

        static EdgarFilesContext()
        {
            Database.SetInitializer<EdgarFilesContext>(new EdgarFilesContextInitializer());
        }

        public EdgarFilesContext() : base("name=AnalystEdgarFiles")
        {
            //http://www.entityframeworktutorial.net/EntityFramework4.3/lazy-loading-with-dbcontext.aspx
            //this.Configuration.LazyLoadingEnabled = true;
            //this.Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<MasterFullIndex> MasterFullIndexes { get; set; }
        public virtual DbSet<MasterDailyIndex> MasterDailyIndexes { get; set; }
        public virtual DbSet<IndexEntry> IndexEntries { get; set; }
        public virtual DbSet<SECForm> SECForms { get; set; }
        public virtual DbSet<SIC> SICs { get; set; }
        public virtual DbSet<Registrant> Registrants { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MasterFullIndex>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("MasterFullIndexes");
            });

            modelBuilder.Entity<MasterDailyIndex>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("MasterDailyIndexes");
            });

            
            modelBuilder.Entity<IndexEntry>()
                .HasRequired(entry => entry.FormType)
                .WithMany()
                .HasForeignKey(entry => entry.FormTypeId)
                .WillCascadeOnDelete(false)
                ;
            
            modelBuilder.Entity<IndexEntry>()
                .HasRequired(entry => entry.Company)
                .WithMany()
                .HasForeignKey(entry => entry.CIK)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IndexEntry>()
                .HasRequired(entry => entry.MasterIndex)
                .WithMany(index => index.Entries)
                .HasForeignKey(entry => entry.MasterIndexId)
                .WillCascadeOnDelete(true);

        }
    }
}
