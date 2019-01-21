
using System.Web.Mvc;

namespace Analyst.Web.Controllers.Frontend
{
    [RoutePrefix("edgar/askedgar")]
    public class AskEdgarController : Controller
    {
        private const string VIEW_ROOT_PATH = "~/Views/Edgar/AskEdgar/";

        public AskEdgarController()
        {
            
        }

        [HttpGet]
        [Route("home")]
        public ActionResult Index()
        {
            return View(VIEW_ROOT_PATH + "AskEdgarHome.cshtml");
        }
    }
}
