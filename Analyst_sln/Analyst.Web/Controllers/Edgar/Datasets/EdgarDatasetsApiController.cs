using Analyst.Domain.Edgar;
using Analyst.Domain.Edgar.Datasets;
using Analyst.Services;
using Analyst.Services.EdgarDatasetServices;
using Analyst.Services.EdgarServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
//using System.Web.Mvc;

namespace Analyst.Web.Controllers
{ 

    [RoutePrefix("edgar/datasets/api")]
    public class EdgarDatasetsApiController : ApiController
    {
        
        private IEdgarService edgarService;
        private IEdgarDatasetService datasetService;

        public EdgarDatasetsApiController(IEdgarService edgarService,IEdgarDatasetService datasetService)
        {
            this.edgarService = edgarService;
            this.datasetService = datasetService;
        }

        [HttpGet]
        [Route("alldatasets")]
        [ResponseType(typeof(IList<EdgarDataset>))]
        public IHttpActionResult GetAllDatasets()
        {
            IList<EdgarDataset> datasets = datasetService.GetDatasets();
            return Ok(datasets);
        }

        [HttpPost]
        [Route("processdataset", Name = "processdataset")]
        public IHttpActionResult ProcessDataset([FromBody]int id)
        {
            datasetService.ProcessDataset(id);
            IList<EdgarDataset> datasets = datasetService.GetDatasets();
            return Ok(datasets);
        }

        [HttpPost]
        [Route("deletedataset",Name ="deletedataset")]
        public IHttpActionResult DeleteDataset(DatasetAndIdParameters parameters)
        {
            datasetService.DeleteDatasetFile(parameters.id, parameters.file);
            IList<EdgarDataset> datasets = datasetService.GetDatasets();
            return Ok(datasets);
        }
       

        public class DatasetAndIdParameters
        {
            public int id { get; set; }
            public string file { get; set; }
        }
    }
}