using Analyst.Services.AnalysisProcesses.ScreenAnalyzeTrade;
using Analyst.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Analyst.Web.Controllers
{
    [RoutePrefix("screenanalyzetrade")]
    public class ScreenAnalyzeTradeController : Controller
    {
        private IExcelManager excelManager;
        public ScreenAnalyzeTradeController(IExcelManager excelManager)
        {
            this.excelManager = excelManager;
        }

        [HttpGet]
        [Route("home")]
        public ActionResult Index()
        {
            ViewBag.Test = "Server time: " + DateTime.Now.ToString();
            return View("Start");
        }

        [HttpGet]
        [Route("screener")]
        public ActionResult Screener()
        {
            ViewBag.Test = "Server time: " + DateTime.Now.ToString();
            return View("Step01_Screener", new ScreenAnalyzeTradeModel());

        }

        [HttpPost]
        [Route("screener_uploadfile")]
        public ActionResult ScreenerUploadFile(HttpPostedFileBase file)
        {
            ScreenAnalyzeTradeModel model = new ScreenAnalyzeTradeModel();
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    DataTable dt = excelManager.ReadExcelAsDatatable(file.InputStream);
                    model.ExcelDataTable = dt;
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return View("Step01_Screener", model);
        }
    }


}