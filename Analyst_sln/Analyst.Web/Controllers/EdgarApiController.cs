using Analyst.Domain.Edgar;
using Analyst.Domain.Edgar.Datasets;
using Analyst.Services;
using Analyst.Services.EdgarDatasetServices;
using Analyst.Services.EdgarServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
//using System.Web.Mvc;

namespace Analyst.Web.Controllers
{ 

    [RoutePrefix("edgar_api")]
    public class EdgarApiController : ApiController
    {
        
        private IEdgarService edgarService;
        private IEdgarDatasetService datasetService;

        public EdgarApiController(IEdgarService edgarService,IEdgarDatasetService datasetService)
        {
            this.edgarService = edgarService;
            this.datasetService = datasetService;
        }

        [HttpGet]
        [Route("datasets/all")]
        [ResponseType(typeof(IList<EdgarDataset>))]
        public IHttpActionResult GetAllDatasets()
        {
            IList<EdgarDataset> datasets = datasetService.GetDatasets();
            return Ok(datasets);

        }

        [Route("datasets/process",Name = "processds")]
        public IHttpActionResult ProcessDataset(int id)
        {
            datasetService.ProcessDataset(id);
            IList<EdgarDataset> datasets = datasetService.GetDatasets();
            return Ok(datasets);
        }

        [Route("datasets/getdetails",Name= "dsdetails")]
        public IHttpActionResult GetDatasetDetails(int id)
        {
            EdgarDataset ds = datasetService.GetDataset(id);
            return Ok(id);
        }

        [HttpGet]
        [Route("secforms")]
        public IHttpActionResult GetSECForms()
        {
            IList<SECForm> forms = edgarService.GetSECForms();
            return Ok(forms);
        }

        [HttpGet]
        [Route("sics")]
        public IHttpActionResult GetSICs()
        {
            IList<SIC> sics = edgarService.GetSICs();
            return Ok(sics);
        }
    }
}