using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialAnalyst.BatchProcesses.EdgarSEC;
using FinancialAnalyst.BatchProcesses.EdgarSEC.DatasetsParsingProcess;
using FinancialAnalyst.BatchProcesses.EdgarSEC.FilesParsingProcess;
using FinancialAnalyst.Common.Entities.EdgarSEC.Repositories;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.DataSources;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces.Edgar;
using FinancialAnalyst.DataAccess.EdgarSEC.Repositories;
using FinancialAnalyst.DataAccess.EdgarSEC.Repositories.BulkRepositories;
using FinancialAnalyst.DataSources;
using FinancialAnalyst.DataSources.Nasdaq;
using FinancialAnalyst.DataSources.Reuters;
using FinancialAnalyst.DataSources.USTreasury;
using FinancialAnalyst.DataSources.Yahoo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FinancialAnalyst.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            //https://stackoverflow.com/questions/29841503/json-serialization-deserialization-in-asp-net-core
            services.AddMvc().AddNewtonsoftJson();

            #region Financial Analyst services/managers/parsers
            services.AddTransient<ICacheManager, FileCacheManager>();

            services.AddTransient<IDataSource, DataSourceManager>();
            services.AddTransient<IPricesDataSource, YahooDataSource>();
            services.AddTransient<IStockDataDataSource, ReutersDataSource>();
            services.AddTransient<IOptionChainDataSource, NasdaqDataSource>();
            services.AddTransient<IFinancialDataSource, ReutersDataSource>();
            services.AddTransient<IRiskFreeRatesDataSource, USTreasuryDataSource>();

            services.AddTransient<IEdgarService, EdgarService>();
            services.AddTransient<IEdgarDatasetParser, EdgarDatasetParser>();
            services.AddTransient<IEdgarFileParser, EdgarFileParser>();
            services.AddTransient<IMasterIndexesParser, MasterIndexesParser>();
            services.AddTransient<IEdgarWebClient, EdgarWebClient>();

            services.AddTransient<IEdgarRepository, EdgarRepository>();
            services.AddTransient<IEdgarFilesRepository, EdgarRepository>();
            services.AddTransient<IEdgarFilesBulkRepository, EdgarFilesBulkRepository>();
            services.AddTransient<IEdgarDatasetsRepository, EdgarRepository>();
            services.AddTransient<IEdgarDatasetsBulkRepository, EdgarDatasetsBulkRepository>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
