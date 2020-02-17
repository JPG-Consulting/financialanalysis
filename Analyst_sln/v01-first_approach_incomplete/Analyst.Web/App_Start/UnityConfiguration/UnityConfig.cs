using Analyst.DBAccess.Contexts;
using Analyst.DBAccess.Repositories;
using Analyst.Services;
using Analyst.Services.AnalysisProcesses.ScreenAnalyzeTrade;
using Analyst.Services.EdgarDatasetServices;
using Analyst.Services.EdgarServices;
using Analyst.Services.EdgarServices.EdgarIndexesServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Unity;

namespace Analyst.Web.App_Start.UnityConfiguration
{
    public class UnityConfig
    {
        //https://stackoverflow.com/questions/11905381/no-parameterless-constructor-defined-for-this-object-exception-while-using-unity
        /*
        ASP.NET MVC and ASP.NET Web API uses two separate dependency resolvers.
        For "regular" MVC controllers which are derives from Controller you need to use the DependencyResolver.SetResolver:

        DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        For the Wep API controllers which are derives form ApiController you need to use the GlobalConfiguration.Configuration.DependencyResolver as in your code.

        So if you plan to use both type of controllers you need to register your container twice.

        There is a good article how to setup Unity for both dependency resolver:

        Dependency Injection in ASP.NET MVC 4 and WebAPI using Unity:
            http://netmvc.blogspot.com.ar/2012/04/dependency-injection-in-aspnet-mvc-4.html
        */
        /////////////////////////////////////////////////////////////////////////////////////////////////////
        //Version sencilla
        //https://www.devtrends.co.uk/blog/using-unity.mvc5-and-unity.webapi-together-in-a-project
        //que necesita: Unity.4.0.1
        //packages\Unity.4.0.1\lib\net45\Microsoft.Practices.Unity.dll 
        //https://stackoverflow.com/questions/35674014/where-is-microsoft-practices-unity-package
        internal static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            RegisterAnalystComponents(container);

            //register the resolver for MVC
            DependencyResolver.SetResolver(new Unity.AspNet.Mvc.UnityDependencyResolver(container));

            //register the resolver for Web API
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.AspNet.WebApi.UnityDependencyResolver(container);
        }

        private static void RegisterAnalystComponents(UnityContainer container)
        {
            container.RegisterInstance<HttpConfiguration>(GlobalConfiguration.Configuration);
            container.RegisterType<IEdgarService, EdgarService>();
            container.RegisterType<IEdgarDatasetService, EdgarDatasetService>();
            container.RegisterType<IAnalystEdgarRepository, AnalystEdgarRepository>();
            container.RegisterType<IAnalystEdgarDatasetsRepository, AnalystEdgarRepository>();
            container.RegisterType<IAnalystEdgarFilesRepository, AnalystEdgarRepository>();
            container.RegisterType<IAnalystEdgarDatasetsBulkRepository, AnalystEdgarDatasetsBulkRepository>();
            container.RegisterType<IAnalystEdgarFilesBulkRepository, AnalystEdgarFilesBulkRepository>();
            
            /*
            container.RegisterType<IEdgarDatasetSubmissionsService, EdgarDatasetSubmissionsService>();
            container.RegisterType<IEdgarDatasetTagService, EdgarDatasetTagService>();
            container.RegisterType<IEdgarDatasetNumService, EdgarDatasetNumService>();
            container.RegisterType<IEdgarDatasetDimensionService, EdgarDatasetDimensionService>();
            container.RegisterType<IEdgarDatasetRenderService, EdgarDatasetRenderService>();
            container.RegisterType<IEdgarDatasetPresentationService, EdgarDatasetPresentationService>();
            container.RegisterType<IEdgarDatasetCalculationService, EdgarDatasetCalculationService>();
            container.RegisterType<IEdgarDatasetTextService, EdgarDatasetTextService>();
            */
            container.RegisterType<IEdgarMasterIndexService, EdgarMasterIndexService>();
            container.RegisterType<IEdgarWebClient, EdgarWebClient>();
            container.RegisterType<IEdgarFileParser, EdgarFileParser>();

            container.RegisterType<IExcelManager, ExcelManager>();
    }
    }
}
 