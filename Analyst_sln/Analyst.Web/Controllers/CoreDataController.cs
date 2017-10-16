using Analyst.Domain.Edgar;
using Analyst.Domain.Edgar.Datasets;
using Analyst.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Analyst.Web.Controllers
{ 

    [RoutePrefix("coredata")]
    public class CoreDataController : Controller
    {
        public IEdgarService Service
        {
            //TODO: implementar inyeccion de dependencias
            get { return new EdgarService(); }
            set { }
        }

        [HttpGet]
        [Route("datasets/all")]
        public ActionResult GetAllDatasets()
        {
            List<EdgarDataset> datasets = Service.GetDatasets();
            return View("Datasets", datasets);

        }

        [HttpGet]
        [Route("datasets/readme")]
        public ActionResult GetReadme()
        {
            return View("readme");
        }

        [HttpGet]
        [Route("secforms")]
        public ActionResult GetSECForms()
        {
            List<SECForm> forms = Service.GetSECForms();
            return View("SECForms", forms);
        }
    }
}