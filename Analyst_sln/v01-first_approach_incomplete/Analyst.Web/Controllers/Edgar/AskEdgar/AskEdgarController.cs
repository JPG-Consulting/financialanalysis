
using Analyst.Services.EdgarServices;
using Analyst.Web.Models;
using System.Web.Mvc;
using System.Linq;
using PagedList;
using Analyst.Domain.Edgar;
using Analyst.Domain;

namespace Analyst.Web.Controllers.Frontend
{
    [RoutePrefix("edgar/askedgar")]
    public class AskEdgarController : Controller
    {
        private const string VIEW_ROOT_PATH = "~/Views/Edgar/AskEdgar/";
        private const string VIEW_HOME = VIEW_ROOT_PATH + "AskEdgarHome.cshtml";
        private const string VIEW_REGISTRANTS = VIEW_ROOT_PATH + "Registrants.cshtml";

        private IEdgarService edgarService;

        public AskEdgarController(IEdgarService edgarService)
        {
            this.edgarService = edgarService;
        }

        [HttpGet]
        [Route("home")]
        public ActionResult Index()
        {
            return View(VIEW_HOME,new AskEdgarModel());
        }

        [HttpGet]
        [Route("getregistrantsby")]
        public ActionResult GetRegistrantsBy(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //Paging
            //https://docs.microsoft.com/es-es/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/sorting-filtering-and-paging-with-the-entity-framework-in-an-asp-net-mvc-application#add-paging

            const int pageSize = 10;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            AskEdgarModel model = new AskEdgarModel();
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter == null ? "":currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            int pageNumber = (page ?? 1);
            int count;
            var query = edgarService.GetRegistrants(sortOrder, searchString, pageSize,out count);
            model.Registrants = query.ToPagedList(pageNumber, pageSize);
            model.PageNumber = pageNumber;
            model.PageCount = count / pageSize + ((count % pageSize) > 0 ? 1 : 0);
            model.Total = count;
            return View(VIEW_REGISTRANTS, model);
        }

        [HttpGet]
        [Route("getfilingsby")]
        public ActionResult GetFilingsBy(int cik)
        {
            AskEdgarModel model = new AskEdgarModel();
            model.Title = "Show filings";
            int? year=null;
            Quarter? quarter=null;
            string sortOrder = "";
            const int pagesize = 10;
            int count;
            IQueryable filings = edgarService.GetFilings(cik, year, quarter, sortOrder, pagesize, out count);
            return View(VIEW_HOME, model);
        }
    }
}
