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

    [RoutePrefix("edgar")]
    public class EdgarController : Controller
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

        [Route("datasets/process",Name = "processds")]
        public ActionResult ProcessDataset(int id)
        {
            EdgarDataset ds = Service.ProcessDataset(id);
            return View("Datasetstatus", ds);
        }

        [Route("datasets/getdetails",Name= "dsdetails")]
        public ActionResult GetDatasetDetails(int id)
        {
            EdgarDataset ds = Service.GetDataset(id);
            return View("DatasetDetail", id);
        }

        [HttpGet]
        [Route("secforms")]
        public ActionResult GetSECForms()
        {
            List<SECForm> forms = Service.GetSECForms();
            return View("SECForms", forms);
        }

        [HttpGet]
        [Route("sics")]
        public ActionResult GetSICs()
        {
            List<SIC> sics = Service.GetSICs();
            return View("SICs", sics);
        }
    }
}