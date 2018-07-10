using Analyst.Domain.Edgar;
using Analyst.Domain.Edgar.Datasets;
using Analyst.Services.EdgarDatasetServices;
using Analyst.Services.EdgarServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Analyst.Web.Controllers.Edgar.Datasets
{
    [RoutePrefix("edgar/datasets")]
    public class EdgarDatasetsController : Controller
    {
        private const string VIEW_ROOT_PATH = "~/Views/Edgar/Datasets/";
        private IEdgarService edgarService;
        private IEdgarDatasetService datasetService;

        public EdgarDatasetsController(IEdgarService edgarService, IEdgarDatasetService datasetService)
        {
            this.edgarService = edgarService;
            this.datasetService = datasetService;
        }

        [HttpGet]
        [Route("home")]
        public ActionResult Index()
        {
            return View(VIEW_ROOT_PATH + "Home.cshtml");
        }


        [HttpGet]
        [Route("readme")]
        public ActionResult Readme()
        {
            return View(VIEW_ROOT_PATH + "readme.cshtml");
        }

        [HttpGet]
        [Route("secforms")]
        public ActionResult GetSecForms()
        {
            IList<SECForm> forms = edgarService.GetSECForms();
            return View(VIEW_ROOT_PATH + "SECForms.cshtml", forms);
        }

        [HttpGet]
        [Route("sics")]
        public ActionResult GetSICs()
        {
            IList<SIC> sics = edgarService.GetSICs();
            return View(VIEW_ROOT_PATH + "SICs.cshtml", sics);
        }

        [HttpGet]
        [Route("registrants")]
        public ActionResult GetRegistrants()
        {
            IList<Registrant> registrants = edgarService.GetCompanies();
            return View(VIEW_ROOT_PATH + "Registrants.cshtml", registrants);
        }

        [HttpGet]
        [Route("process")]
        public ActionResult Process()
        {
            return View(VIEW_ROOT_PATH + "ProcessDatasets.cshtml");
        }

    }
}