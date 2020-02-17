using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Analyst.Web.Controllers
{
    [RoutePrefix("")]
    public class MainController : Controller
    {
        [HttpGet]
        [Route("home")]
        [Route("index")]
        [Route("")]
        public ActionResult Index()
        {
            return View("~/Views/Index.cshtml");
        }
    }
}