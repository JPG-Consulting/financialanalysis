using Analyst.Domain.Edgar;
using Analyst.Domain.Edgar.Datasets;
using Analyst.Services;
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

        public EdgarController(IEdgarService edgarService,IEdgarDatasetService datasetService)
        {
            this.edgarService = edgarService;
            this.datasetService = datasetService;
        }

        [HttpGet]
        [Route("datasets/all")]
        public ActionResult GetAllDatasets()
        {
            IList<EdgarDataset> datasets = datasetService.GetDatasets();
            return View("Datasets", datasets);

        }

        [HttpGet]
        [Route("datasets/readme")]
        public ActionResult GetReadme()
        {
            return View("readme");
        }

        [Route("datasets/process",Name = "processds")]
        public ActionResult ProcessDataset(int id)
        {
            datasetService.ProcessDataset(id);
            IList<EdgarDataset> datasets = datasetService.GetDatasets();
            return View("Datasets", datasets);
        }

        [Route("datasets/getdetails",Name= "dsdetails")]
        public ActionResult GetDatasetDetails(int id)
        {
            EdgarDataset ds = datasetService.GetDataset(id);
            return View("DatasetDetail", id);
        }

        [HttpGet]
        [Route("secforms")]
        public ActionResult GetSECForms()
        {
            IList<SECForm> forms = edgarService.GetSECForms();
            return View("SECForms", forms);
        }

        [HttpGet]
        [Route("sics")]
        public ActionResult GetSICs()
        {
            IList<SIC> sics = edgarService.GetSICs();
            return View("SICs", sics);
        }
    }
}