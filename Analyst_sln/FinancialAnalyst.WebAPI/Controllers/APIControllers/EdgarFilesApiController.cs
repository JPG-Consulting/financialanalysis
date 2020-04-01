using FinancialAnalyst.BatchProcesses.EdgarSEC.FilesParsingProcess;
using FinancialAnalyst.Common.Entities.EdgarSEC.Indexes;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using FinancialAnalyst.WebAPI.Models;

namespace Analyst.Web.Controllers.Edgar.Files
{
    /// <summary>
    /// Indexes to all public filings are are available from 1994Q3 through the present and located in the following browsable directories:
    /// * /Archives/edgar/daily-index — daily index files through the current year;
    /// * /Archives/edgar/full-index — Full indexes offer a "bridge" between quarterly and daily indexes, 
    ///   compiling filings from the beginning of the current quarter through the previous business day.At the end of the quarter, 
    ///   the full index is rolled into a static quarterly index.
    /// </summary>
    [Route("edgar/files/api")]
    public class EdgarFilesApiController : ControllerBase
    {
        private IMasterIndexesParser indexesParser;
        public EdgarFilesApiController(IMasterIndexesParser indexesParser)
        {
            this.indexesParser = indexesParser;
        }
            

        [HttpGet]
        [Route("dailyindex")]
        public ActionResult<MasterIndex> GetDailyIndex(ushort year,ushort quarter, uint date)
        {
            MasterIndex index = indexesParser.ProcessDailyIndex(year, quarter, date);
            return Ok(index);
        }

        [HttpGet]
        [Route("fullindexes")]
        public ActionResult<IList<MasterIndex>> GetFullIndexes()
        {
            IList<MasterIndex> indexes = indexesParser.GetFullIndexes();
            return Ok(indexes);
        }

        [HttpPost]
        [Route("processfullindex")]
        public ActionResult<IList<MasterIndex>> ProcessFullIndex(ProcessFullIndexParameters param)
        {
            indexesParser.ProcessFullIndex((ushort)param.year, (ushort)param.quarter);
            IList<MasterIndex> indexes = indexesParser.GetFullIndexes();
            return Ok(indexes);
        }

        
    }
}
