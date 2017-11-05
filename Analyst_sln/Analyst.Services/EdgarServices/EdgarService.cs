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
        List<SECForm> GetSECForms();
        List<SIC> GetSICs();
    }

    public class EdgarService : IEdgarService
    {
        private IAnalystRepository repository;

        public EdgarService(IAnalystRepository repository)
        {
            this.repository = repository;
        }


        public List<SECForm> GetSECForms()
        {
            List<SECForm> forms = repository.GetSECForms();
            return forms;
        }

        public List<SIC> GetSICs()
        {
            List<SIC> sics = repository.GetSICs();
            return sics;
        }


        
    }

    
   
}
