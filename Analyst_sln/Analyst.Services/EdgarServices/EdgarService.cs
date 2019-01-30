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
using Analyst.DBAccess.Repositories;

namespace Analyst.Services.EdgarServices
{
    public interface IEdgarService
    {
        IList<SECForm> GetSECForms();
        IList<SIC> GetSICs();
        IQueryable<Registrant> GetRegistrants(string sortOrder, string searchString, int pagesize,out int total);
        IQueryable GetFilings(int cik, int? year, Quarter? quarter, string sortOrder, int pagesize, out int count);
    }

    public class EdgarService : IEdgarService
    {
        private IAnalystEdgarDatasetsRepository datasetsRepo;
        private IAnalystEdgarRepository edgarRepo;

        public EdgarService(IAnalystEdgarRepository edgarRepo, IAnalystEdgarDatasetsRepository datasetsRepo)
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
