using Analyst.DBAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.DBAccess.Contexts
{
    //public class EdgarDatasetsContextInitializer : DropCreateDatabaseIfModelChanges<AnalystContext>
    internal class EdgarContextInitializer: CreateDatabaseIfNotExists<EdgarContext>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("EdgarDatasetsContextInitializer");
        protected override void Seed(EdgarContext context)
        {
            try
            {
                log.Info("Starting Seed");
                base.Seed(context);
                log.Info("Seed base called");
                /*
                Tags are case sensitive, example:
                tag	version	custom	abstract	datatype	iord	crdr	tlabel	doc
                EchostarXviMember	0001558370-16-009751	1	1	member			Echostar Xvi [Member]	Represents the information pertaining to satellites assets owned by the entity, EchoStar XVI.
                EchoStarXVIMember	0001558370-16-009751	1	1	member			Echo Star X V I [Member]	Represents information pertaining to satellites assets leased from EchoStar, Echo Star XVI, which are accounted for as operating leases.

                And EF isn't case sensitive by default
                https://stackoverflow.com/questions/3843060/linq-to-entities-case-sensitive-comparison
                https://milinaudara.wordpress.com/2015/02/04/case-sensitive-search-using-entity-framework-with-custom-annotation/

                //this works for sql server and it has to be run before the index is created
                ALTER TABLE EdgarDatasetsTag 
                ALTER COLUMN Tag VARCHAR(10) 
                COLLATE SQL_Latin1_General_CP1_CS_AS
                */

                //Solution
                //http://www.entityframeworktutorial.net/code-first/database-initialization-strategy-in-code-first.aspx

                context.Database.ExecuteSqlCommand("ALTER TABLE EdgarDatasetTags ALTER COLUMN Tag NVARCHAR(256) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL");
                log.Info(" created");
                context.Database.ExecuteSqlCommand("ALTER TABLE EdgarDatasetTags ALTER COLUMN Version NVARCHAR(20) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL");
                log.Info(" created");
                context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IX_TagVersion ON EdgarDatasetTags (Tag, Version,DatasetId)");
                log.Info(" created");
                context.Database.ExecuteSqlCommand(GetTextScript("alter column ADSH.sql"));
                log.Info(" created");

                List<string> scripts = new List<string>();
                context.Database.ExecuteSqlCommand(GetTextScript("create GET_MISSING_LINE_NUMBERS.sql"));
                log.Info("GET_MISSING_LINE_NUMBERS created");
                context.Database.ExecuteSqlCommand(GetTextScript("create SP_DISABLE_PRESENTATION_INDEXES.sql"));
                log.Info("SP_DISABLE_PRESENTATION_INDEXES created");
                context.Database.ExecuteSqlCommand(GetTextScript("create SP_EDGARDATASETCALC_INSERT.sql"));
                log.Info("SP_EDGARDATASETCALC_INSERT created");
                context.Database.ExecuteSqlCommand(GetTextScript("create SP_EDGARDATASETDIMENSIONS_INSERT.sql"));
                log.Info("SP_EDGARDATASETDIMENSIONS_INSERT created");
                context.Database.ExecuteSqlCommand(GetTextScript("create SP_EDGARDATASETNUMBER_INSERT.sql"));
                log.Info("SP_EDGARDATASETNUMBER_INSERT created");
                context.Database.ExecuteSqlCommand(GetTextScript("create SP_EDGARDATASETPRESENTATIONS_INSERT.sql"));
                log.Info("SP_EDGARDATASETPRESENTATIONS_INSERT created");
                context.Database.ExecuteSqlCommand(GetTextScript("create SP_EDGARDATASETRENDERS_INSERT.sql"));
                log.Info("SP_EDGARDATASETRENDERS_INSERT created");
                context.Database.ExecuteSqlCommand(GetTextScript("create SP_EDGARDATASETSUBMISSIONS_INSERT.sql"));
                log.Info("SP_EDGARDATASETSUBMISSIONS_INSERT created");
                context.Database.ExecuteSqlCommand(GetTextScript("create SP_EDGARDATASETTAGS_INSERT.sql"));
                log.Info("SP_EDGARDATASETTAGS_INSERT created");
                context.Database.ExecuteSqlCommand(GetTextScript("create SP_EDGARDATASETTEXT_INSERT.sql"));
                log.Info("SP_EDGARDATASETTEXT_INSERT created");
                context.Database.ExecuteSqlCommand(GetTextScript("create SP_GET_CALCULATIONS_KEYS.sql"));
                log.Info("SP_GET_CALCULATIONS_KEYS created");
                context.Database.ExecuteSqlCommand(GetTextScript("create SP_GET_DIMENSIONS_KEYS.sql"));
                log.Info("SP_GET_DIMENSIONS_KEYS created");
                context.Database.ExecuteSqlCommand(GetTextScript("create SP_GET_NUMBER_KEYS.sql"));
                log.Info("SP_GET_NUMBER_KEYS created");
                context.Database.ExecuteSqlCommand(GetTextScript("create SP_GET_PRESENTATION_KEYS.sql"));
                log.Info("SP_GET_PRESENTATION_KEYS created");
                context.Database.ExecuteSqlCommand(GetTextScript("create SP_GET_RENDER_KEYS.sql"));
                log.Info("SP_GET_RENDER_KEYS created");
                context.Database.ExecuteSqlCommand(GetTextScript("create SP_GET_SUBMISSIONS_KEYS.sql"));
                log.Info("SP_GET_SUBMISSIONS_KEYS created");
                context.Database.ExecuteSqlCommand(GetTextScript("create SP_GET_TAGS_KEYS.sql"));
                log.Info("SP_GET_TAGS_KEYS created");
                context.Database.ExecuteSqlCommand(GetTextScript("create SP_GET_TEXT_KEYS.sql"));
                log.Info("SP_GET_TEXT_KEYS created");
                context.Database.ExecuteSqlCommand(GetTextScript("create table LOG.sql"));
                log.Info("LOG created");
                context.Database.ExecuteSqlCommand(GetTextScript("create table numbers.sql"));
                log.Info("numbers created");
                IAnalystEdgarRepository repo = new AnalystEdgarRepository(context);
                log.Info("Loading initial data");
                EdgarInitialLoader.LoadInitialData(repo);
                log.Info("SecForms and SICs loaded");
                EdgarInitialLoader.LoadInitialDatasets(repo);
                log.Info("Datasets loaded");
                EdgarInitialLoader.LoadInitialFullIndexes(repo);
                log.Info("Indexes loaded");

                log.Info("Seed end");
            }
            catch(Exception ex)
            {
                log.Fatal("Error seeding Datasets database: " + ex.Message, ex);
                throw ex;
            }
        }

        private string GetTextScript(string scriptFileName)
        {
            StreamReader sr = File.OpenText(ConfigurationManager.AppSettings["scripts_folder"] + "\\" + scriptFileName);
            string text = sr.ReadToEnd();
            sr.Close();
            return text;
        }
    }
}
