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
        IList<Registrant> GetRegistrants();
        IList<Registrant> GetCompanies();
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
            IList<SECForm> forms = datasetsRepo.Get<SECForm>();
            return forms;
        }

        public IList<SIC> GetSICs()
        {
            IList<SIC> sics = datasetsRepo.Get<SIC>();
            return sics;
        }

        public IList<Registrant> GetRegistrants()
        {
            //TODO: Implement discriminator
            //https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/implementing-inheritance-with-the-entity-framework-in-an-asp-net-mvc-application
            //IList<Registrant> registrants = repository.Get<Registrant>();//TODO: Registrant shoul implement IEdgarEntity
            //return registrants;
            throw new NotImplementedException();
        }

        public IList<Registrant> GetCompanies()
        {
            //TODO: Implement discriminator
            //https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/implementing-inheritance-with-the-entity-framework-in-an-asp-net-mvc-application
            IList<Registrant> companies = edgarRepo.GetCompanies();
            return companies;

        }


    }

    
   
}
