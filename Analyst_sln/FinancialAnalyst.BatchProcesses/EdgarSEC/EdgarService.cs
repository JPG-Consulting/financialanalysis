using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;
using FinancialAnalyst.Common.Entities.EdgarSEC;
using FinancialAnalyst.Common.Entities;
using FinancialAnalyst.Common.Entities.EdgarSEC.Repositories;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.Edgar;

namespace FinancialAnalyst.BatchProcesses.EdgarSEC
{
    public class EdgarService : IEdgarService
    {
        private IEdgarDatasetsRepository datasetsRepo;
        private IEdgarRepository edgarRepo;

        public EdgarService(IEdgarRepository edgarRepo, IEdgarDatasetsRepository datasetsRepo)
        {
            this.datasetsRepo = datasetsRepo;
            this.edgarRepo = edgarRepo;
        }


        public IList<SECForm> GetSECForms()
        {
            IList<SECForm> forms = edgarRepo.Get<SECForm>();
            return forms;
        }

        public IList<SIC> GetSICs()
        {
            IList<SIC> sics = edgarRepo.Get<SIC>();
            return sics;
        }

        public IQueryable<Registrant> GetRegistrants(string sortOrder, string searchString, int pagesize, out int total)
        {
            return datasetsRepo.GetRegistrants(sortOrder, searchString,  pagesize, out total);
        }

        public IQueryable GetFilings(int cik, int? year, Quarter? quarter, string sortOrder, int pagesize, out int count)
        {
            return datasetsRepo.GetFilings(cik, year, quarter, sortOrder, pagesize, out count);
        }
    }

    
   
}
