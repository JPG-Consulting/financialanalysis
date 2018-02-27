using Analyst.Domain.Edgar;
using Analyst.Services.EdgarServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Analyst.Web.Controllers.Frontend
{
    [RoutePrefix("askedgarapi")]
    public class EdgarApiController : ApiController
    {
        private IEdgarService edgarService;

        public EdgarApiController(IEdgarService edgarService)
        {
            this.edgarService = edgarService;
        }

        [HttpGet]
        [Route("companies")]
        [ResponseType(typeof(IList<Registrant>))]
        public IHttpActionResult GetCompanies()
        {
            IList<Registrant> registrants = edgarService.GetCompanies();
            return Ok(registrants);
        }
    }
}
