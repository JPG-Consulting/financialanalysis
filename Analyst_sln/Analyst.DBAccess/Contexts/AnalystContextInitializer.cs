using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.DBAccess.Contexts
{
    public class AnalystContextInitializer : DropCreateDatabaseIfModelChanges<AnalystContext>
    {
        protected override void Seed(AnalystContext context)
        {
            base.Seed(context);

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

            context.Database.ExecuteSqlCommand("ALTER TABLE EdgarDatasetTags ALTER COLUMN Tag VARCHAR(256) NOT NULL COLLATE SQL_Latin1_General_CP1_CS_AS");
            context.Database.ExecuteSqlCommand("ALTER TABLE EdgarDatasetTags ALTER COLUMN Version VARCHAR(20) NOT NULL COLLATE SQL_Latin1_General_CP1_CS_AS");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IX_TagVersion ON EdgarDatasetTags (Tag, Version)");

            //TODO: Llamar a los SP de la carpeta scripts

            InitialLoader.LoadInitialData(new AnalystRepository(context));
        }
    }
}
