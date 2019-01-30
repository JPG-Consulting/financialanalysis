using Analyst.Domain.Edgar.Indexes;
using Analyst.Services.EdgarServices.EdgarIndexesServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Analyst.Web.Controllers.Edgar.Files
{
    /// <summary>
    /// Indexes to all public filings are are available from 1994Q3 through the present and located in the following browsable directories:
    /// * /Archives/edgar/daily-index — daily index files through the current year;
    /// * /Archives/edgar/full-index — Full indexes offer a "bridge" between quarterly and daily indexes, 
    ///   compiling filings from the beginning of the current quarter through the previous business day.At the end of the quarter, 
    ///   the full index is rolled into a static quarterly index.
    /// </summary>
    [RoutePrefix("edgar/files/api")]
    public class EdgarFilesApiController : ApiController
    {
        private IEdgarMasterIndexService indexService;
        public EdgarFilesApiController(IEdgarMasterIndexService indexService)
        {
            this.indexService = indexService;
        }
            

        [HttpGet]
        [Route("dailyindex")]
        public IHttpActionResult GetDailyIndex(ushort year,ushort quarter, uint date)
        {
            MasterIndex index = indexService.ProcessDailyIndex(year, quarter, date);
            return Ok(index);
        }

        [HttpGet]
        [Route("fullindexes")]
        [ResponseType(typeof(IList<MasterIndex>))]
        public IHttpActionResult GetFullIndexes()
        {
            IList<MasterIndex> indexes = indexService.GetFullIndexes();
            return Ok(indexes);
        }

        [HttpPost]
        [Route("processfullindex")]
        public IHttpActionResult ProcessFullIndex(ProcessFullIndexParameter param)
        {
            indexService.ProcessFullIndex((ushort)param.year, (ushort)param.quarter);
            IList<MasterIndex> indexes = indexService.GetFullIndexes();
            return Ok(indexes);
        }

        public class ProcessFullIndexParameter
        {
            public int year { get; set; }
            public int quarter { get; set; }
        }
    }
}
