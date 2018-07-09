using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Analyst.Web.Controllers
{
    [RoutePrefix("screenanalyzetrade")]
    public class ScreenAnalyzeTradeController : Controller
    {

        [HttpGet]
        [Route("home")]
        public ActionResult Index()
        {
            ViewBag.asdf = "asdfasdf";
            return View("Start");
        }
    }
}