using FinancialAnalyst.Common.Entities.Portfolios;
using FinancialAnalyst.Common.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinancialAnalyst.DataAccess.Portfolios
{
    public class PortfoliosInitializer
    {
        public static void Initialize(PortfoliosContext context)
        {
            context.Database.Migrate();

            context.Database.EnsureCreated();

            FillData(context);
        }

        private static void FillData(PortfoliosContext context)
        {
            User defaultUser = context.Users.Include(u => u.Portfolios).Where(u => u.UserName == "sgastia").SingleOrDefault();
            if (defaultUser == null)
            {
                defaultUser = new Common.Entities.Users.User()
                {
                    UserName = "sgastia",
                    Email = "sgastia@gmail.com",
                };
                context.Users.Add(defaultUser);
                context.SaveChanges();
            }

            string name = "Warren Buffet";
            if (defaultUser.Portfolios.Where(p => p.Name == name).Any() == false)
            {
                Portfolio portfolio = Portfolio.From(name, new string[] {
                    "AAL","AAPL","AMZN","AXP","AXTA","BAC","BIIB","BK","CHTR","COST","DAL","DVA","GL","GM","GS","JNJ","JPM",
                    "KHC","KO","KR","LBTYA","LBTYK","LILA","LILAK","LSXMA","LSXMK","LUV","MA","MCO","MDLZ","MTB","OXY","PG","PNC","PSX",
                    "RH","QSR","SIRI","SPY","STNE","STOR","SU","SYF","TEVA","TRV","UAL","UPS","USB","V","VOO","VRSN","WFC",});
                portfolio.UserId = defaultUser.Id;
                context.Add(portfolio);
            }

            name = "ETF";
            if (defaultUser.Portfolios.Where(p => p.Name == name).Any() == false)
            {
                Portfolio portfolio = Portfolio.From(name, new string[] { "TQQQ", "SPXL", "TNA", "MIDU", "VNQ", "GEX", });
                portfolio.UserId = defaultUser.Id;
                context.Add(portfolio);
            }

            name = "Big Technologies";
            if (defaultUser.Portfolios.Where(p => p.Name == name).Any() == false)
            {
                Portfolio portfolio = Portfolio.From(name, new string[] { "AAPL", "AMZN", "CSCO", "GOOGL", "IBM", "MSFT", "ORCL", });
                portfolio.UserId = defaultUser.Id;
                context.Add(portfolio);
            }
        }
    }
}
