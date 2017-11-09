using Analyst.DBAccess;
using Analyst.DBAccess.Contexts;
using Analyst.Domain;
using Analyst.Domain.Edgar.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyst.Domain.Edgar;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;

namespace Analyst.Services.EdgarServices
{
    public interface IEdgarService
    {
        IList<SECForm> GetSECForms();
        IList<SIC> GetSICs();
    }

    public class EdgarService : IEdgarService
    {
        private IAnalystRepository repository;

        public EdgarService(IAnalystRepository repository)
        {
            this.repository = repository;
        }


        public IList<SECForm> GetSECForms()
        {
            IList<SECForm> forms = repository.Get<SECForm>();
            return forms;
        }

        public IList<SIC> GetSICs()
        {
            IList<SIC> sics = repository.Get<SIC>();
            return sics;
        }


        
    }

    
   
}
