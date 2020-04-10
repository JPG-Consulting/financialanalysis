using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialAnalyst.DataAccess.Portfolios;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FinancialAnalyst.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            CreateDatabaseIfNotExists(host);

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void CreateDatabaseIfNotExists(IHost host)
        {
            PortfoliosContext context;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    context = services.GetRequiredService<PortfoliosContext>();
                    PortfoliosInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    //https://docs.microsoft.com/es-es/aspnet/core/fundamentals/logging/?view=aspnetcore-3.1

                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");

                    throw ex;
                }
            }
        }
    }
}
