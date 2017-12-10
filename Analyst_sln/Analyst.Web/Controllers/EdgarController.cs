using Analyst.Domain.Edgar;
using Analyst.Domain.Edgar.Datasets;
using Analyst.Services.EdgarDatasetServices;
using Analyst.Services.EdgarServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Analyst.Web.Controllers
{
    [RoutePrefix("edgar")]
    public class EdgarController : Controller
    {

        private IEdgarService edgarService;
        private IEdgarDatasetService datasetService;

        public EdgarController(IEdgarService edgarService, IEdgarDatasetService datasetService)
        {
            this.edgarService = edgarService;
            this.datasetService = datasetService;
        }

        [HttpGet]
        [Route("home")]
        public ActionResult Index()
        {
            return View("Home");
        }


        [HttpGet]
        [Route("readme")]
        public ActionResult Readme()
        {
            return View("readme");
        }

        [HttpGet]
        [Route("secforms")]
        public ActionResult GetSecForms()
        {
            IList<SECForm> forms = edgarService.GetSECForms();
            return View("SECForms",forms);
        }

        [HttpGet]
        [Route("sics")]
        public ActionResult GetSICs()
        {
            IList<SIC> sics = edgarService.GetSICs();
            return View("SICs",sics);
        }

        [HttpGet]
        [Route("datasets/all")]
        public ActionResult GetDatasets()
        {
            IList<EdgarDataset> datasets = datasetService.GetDatasets();
            return View("Datasets",datasets);
        }
    }
}