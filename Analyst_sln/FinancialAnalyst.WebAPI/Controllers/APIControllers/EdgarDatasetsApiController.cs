using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FinancialAnalyst.Common.Entities.EdgarSEC.Datasets;
using FinancialAnalyst.WebAPI.Models;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.Edgar;

namespace FinancialAnalyst.WebAPI.Controllers.APIControllers
{ 

    [Route("edgar/datasets/api")]
    [ApiController]
    public class EdgarDatasetsApiController : ControllerBase
    {
        
        private IEdgarService edgarService;
        private IEdgarDatasetParser edgarDatasetParser;

        public EdgarDatasetsApiController(IEdgarService edgarService, IEdgarDatasetParser edgarDatasetParser)
        {
            this.edgarService = edgarService;
            this.edgarDatasetParser = edgarDatasetParser;
        }

        [HttpGet]
        [Route("alldatasets")]
        public ActionResult<IList<EdgarDataset>> GetAllDatasets()
        {
            IList<EdgarDataset> datasets = edgarDatasetParser.GetDatasets();
            return Ok(datasets);
        }

        [HttpPost]
        [Route("processdataset", Name = "processdataset")]
        public ActionResult<IList<EdgarDataset>> ProcessDataset([FromBody]int id)
        {
            edgarDatasetParser.ProcessDataset(id);
            IList<EdgarDataset> datasets = edgarDatasetParser.GetDatasets();
            return Ok(datasets);
        }

        [HttpPost]
        [Route("deletedataset",Name ="deletedataset")]
        public ActionResult<IList<EdgarDataset>> DeleteDataset(DatasetParameters parameters)
        {
            edgarDatasetParser.DeleteDatasetFile(parameters.id, parameters.file);
            IList<EdgarDataset> datasets = edgarDatasetParser.GetDatasets();
            return Ok(datasets);
        }
       


    }
}