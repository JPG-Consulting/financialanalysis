using Analyst.Domain.Edgar.Datasets;
using Analyst.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Analyst.Web.Controllers
{
    public class DatasetsController : Controller
    {
        public IEdgarService Service
        {
            //TODO: implementar inyeccion de dependencias
            get { return new EdgarService(); }
            set { }
        }

        // GET: Datasets
        public ActionResult Index()
        {
            List<EdgarDataset> datasets = Service.GetDatasets();
            return View("DatasetsView", datasets);

        }
    }
}