using FinancialAnalyst.Common.Entities.EdgarSEC;
using FinancialAnalyst.Common.Entities.EdgarSEC.Datasets;
using FinancialAnalyst.Common.Entities.EdgarSEC.Indexes;
using Microsoft.EntityFrameworkCore;
using System;


namespace FinancialAnalyst.BatchProcesses.DB.Contexts
{
    internal class EdgarContext : DbContext
    {
        //https://docs.microsoft.com/en-us/ef/core/get-started/?tabs=netcore-cli

        static EdgarContext()
        {
            //Database.SetInitializer<EdgarContext>(new EdgarContextInitializer());
        }

        /*
        public EdgarContext() : base("name=AnalystEdgar")
        {
            //http://www.entityframeworktutorial.net/EntityFramework4.3/lazy-loading-with-dbcontext.aspx
            //this.Configuration.LazyLoadingEnabled = true;
            //this.Configuration.ProxyCreationEnabled = false;
        }
        */

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

        public virtual DbSet<MasterIndex> MasterIndexes { get; set; }
        public virtual DbSet<IndexEntry> IndexEntries { get; set; }

        public virtual DbSet<EdgarTuple> EdgarTuples { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            //https://stackoverflow.com/questions/5559043/entity-framework-code-first-two-foreign-keys-from-same-table

            //codigo anterior (y es el mismo para todos):
            //modelBuilder.Entity<EdgarDatasetCalculation>()
            //    .HasRequired(calc => calc.ParentTag)
            //    .WithMany(tag => tag.ParentCalculations)
            //    .HasForeignKey(calc => calc.ParentTagId)
            //    .WillCascadeOnDelete(false)
            //    ;

            modelBuilder.Entity<EdgarDatasetCalculation>()
                .HasOne(calc => calc.ParentTag)
                .WithMany(tag => tag.ParentCalculations)
                .IsRequired()
                .HasForeignKey(calc => calc.ParentTagId)
                .OnDelete(DeleteBehavior.Cascade);
            ;


            modelBuilder.Entity<EdgarDatasetCalculation>()
                .HasOne(calc => calc.ChildTag)
                .WithMany(tag => tag.ChildCalculations)
                .IsRequired()
                .HasForeignKey(calc => calc.ChildTagId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EdgarDatasetPresentation>()
                .HasOne(pre => pre.Render)
                .WithMany(ren => ren.Presentations)
                .IsRequired()
                .HasForeignKey(pre => pre.RenderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EdgarDatasetPresentation>()
                .HasOne(p => p.Tag)
                .WithMany(t => t.Presentations)
                .IsRequired()
                .HasForeignKey(p => p.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EdgarDatasetRender>()
                .HasOne(ren => ren.Submission)
                .WithMany(sub => sub.Renders)
                .IsRequired()
                .HasForeignKey(ren => ren.SubmissionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EdgarDatasetNumber>()
                .HasOne(n => n.Dimension)
                .WithMany(d => d.Numbers)
                .IsRequired()
                .HasForeignKey(n => n.DimensionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EdgarDatasetNumber>()
                .HasOne(n => n.Tag)
                .WithMany(t => t.Numbers)
                .IsRequired()
                .HasForeignKey(n => n.TagId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<EdgarDatasetSubmission>()
                .HasOne(sub => sub.Registrant)
                .WithMany()
                .IsRequired()
                .HasForeignKey(sub => sub.RegistrantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EdgarDatasetSubmission>()
                .HasOne(sub => sub.Form)
                .WithMany()
                .IsRequired()
                .HasForeignKey(sub => sub.SECFormId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EdgarDatasetCalculation>()
                .HasOne(fk => fk.Submission)
                .WithMany(sub => sub.Calculations)
                .IsRequired()
                .HasForeignKey(fk => fk.SubmissionId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<EdgarDatasetNumber>()
                .HasOne(fk => fk.Submission)
                .WithMany(sub => sub.Numbers)
                .IsRequired()
                .HasForeignKey(fk => fk.SubmissionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EdgarDatasetPresentation>()
                .HasOne(fk => fk.Submission)
                .WithMany(sub => sub.Presentations)
                .IsRequired()
                .HasForeignKey(fk => fk.SubmissionId)
                .OnDelete(DeleteBehavior.Cascade);

            /*
            modelBuilder.Entity<EdgarDatasetPresentation>()
                .HasOne(fk => fk.Number)
                .WithMany(n => n.Presentations)
                .IsRequired()
                .HasForeignKey(fk => fk.NumberId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EdgarDatasetPresentation>()
                .HasOne(fk => fk.Text)
                .WithMany(sub => sub.Presentations)
                .IsRequired()
                .HasForeignKey(fk => fk.TextId)
                .OnDelete(DeleteBehavior.Cascade);
            */

            modelBuilder.Entity<EdgarDatasetText>()
                .HasOne(fk => fk.Submission)
                .WithMany(sub => sub.Texts)
                .IsRequired()
                .HasForeignKey(fk => fk.SubmissionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EdgarDatasetText>()
                .HasOne(fk => fk.Tag)
                .WithMany(t => t.Texts)
                .IsRequired()
                .HasForeignKey(fk => fk.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EdgarDatasetText>()
                .HasOne(fk => fk.Dimension)
                .WithMany(d => d.Texts)
                .IsRequired()
                .HasForeignKey(fk => fk.DimensionId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<IndexEntry>()
                .HasOne(entry => entry.FormType)
                .WithMany()
                .IsRequired()
                .HasForeignKey(entry => entry.FormTypeId)
                .OnDelete(DeleteBehavior.Cascade);
            ;

            modelBuilder.Entity<IndexEntry>()
                .HasOne(entry => entry.Company)
                .WithMany()
                .IsRequired()
                .HasForeignKey(entry => entry.CIK)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<IndexEntry>()
                .HasOne(entry => entry.MasterIndex)
                .WithMany(index => index.Entries)
                .IsRequired()
                .HasForeignKey(entry => entry.MasterIndexId)
                .OnDelete(DeleteBehavior.Cascade);

        }


        
    }
    

}
