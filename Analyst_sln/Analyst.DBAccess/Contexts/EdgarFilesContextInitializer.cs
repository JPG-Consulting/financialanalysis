using Analyst.DBAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.DBAccess.Contexts
{
    //internal class EdgarFilesContextInitializer : CreateDatabaseIfNotExists<EdgarFilesContext>
    internal class EdgarFilesContextInitializer: DropCreateDatabaseIfModelChanges<EdgarFilesContext>
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected override void Seed(EdgarFilesContext context)
        {
            base.Seed(context);

            IAnalystEdgarFilesRepository repo = new AnalystEdgarFilesEFRepository(context);
            EdgarInitialLoader.LoadInitialData(repo);
            EdgarInitialLoader.LoadInitialFullIndexes(repo);
        }
    }
}
