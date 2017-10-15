﻿using Analyst.Domain.Edgar.Datasets;
using Analyst.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Analyst.Web.Controllers
{
    public class DatasetsController : Controller
    {
        public EdgarService Service { get; set; }

        // GET: Datasets
        public ActionResult Index()
        {
            Service = new EdgarService();
            List<EdgarDataset> datasets = Service.GetDatasets();
            return View("DatasetsView", datasets);
            //return View(datasets);
        }
    }
}