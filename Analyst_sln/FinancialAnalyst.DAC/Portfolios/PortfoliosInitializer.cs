using FinancialAnalyst.Common.Entities.Assets;
using FinancialAnalyst.Common.Entities.Portfolios;
using FinancialAnalyst.Common.Entities.Users;
using FinancialAnalyst.Common.Interfaces.ServiceLayerInterfaces;
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

            //context.Database.EnsureCreated();

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
                    Portfolios = new List<Portfolio>(),
                };
                context.Users.Add(defaultUser);
                context.SaveChanges();
            }

            //https://www.cnbc.com/berkshire-hathaway-portfolio/
            string name = "Warren Buffet";
            if (defaultUser.Portfolios.Where(p => p.Name == name).Any() == false)
            {
                context.CreatePortfolio(defaultUser, name,true, new Tuple<string, AssetClass>[] {
                    new Tuple<string,AssetClass>("AAL", AssetClass.Stock),
                    new Tuple<string,AssetClass>("AAPL", AssetClass.Stock),
                    new Tuple<string,AssetClass>("AMZN", AssetClass.Stock),
                    new Tuple<string,AssetClass>("AXP", AssetClass.Stock),
                    new Tuple<string,AssetClass>("AXTA", AssetClass.Stock),
                    new Tuple<string,AssetClass>("BAC", AssetClass.Stock),
                    new Tuple<string,AssetClass>("BIIB", AssetClass.Stock),
                    new Tuple<string,AssetClass>("BK", AssetClass.Stock),
                    new Tuple<string,AssetClass>("CHTR", AssetClass.Stock),
                    new Tuple<string,AssetClass>("COST", AssetClass.Stock),
                    new Tuple<string,AssetClass>("DAL", AssetClass.Stock),
                    new Tuple<string,AssetClass>("DVA", AssetClass.Stock),
                    new Tuple<string,AssetClass>("GL", AssetClass.Stock),
                    new Tuple<string,AssetClass>("GM", AssetClass.Stock),
                    new Tuple<string,AssetClass>("GS", AssetClass.Stock),
                    new Tuple<string,AssetClass>("JNJ", AssetClass.Stock),
                    new Tuple<string,AssetClass>("JPM", AssetClass.Stock),
                    new Tuple<string,AssetClass>("KHC", AssetClass.Stock),
                    new Tuple<string,AssetClass>("KO", AssetClass.Stock),
                    new Tuple<string,AssetClass>("KR", AssetClass.Stock),
                    new Tuple<string,AssetClass>("LBTYA", AssetClass.Stock),
                    new Tuple<string,AssetClass>("LBTYK", AssetClass.Stock),
                    new Tuple<string,AssetClass>("LILA", AssetClass.Stock),
                    new Tuple<string,AssetClass>("LILAK", AssetClass.Stock),
                    new Tuple<string,AssetClass>("LSXMA", AssetClass.Stock),
                    new Tuple<string,AssetClass>("LSXMK", AssetClass.Stock),
                    new Tuple<string,AssetClass>("LUV", AssetClass.Stock),
                    new Tuple<string,AssetClass>("MA", AssetClass.Stock),
                    new Tuple<string,AssetClass>("MCO", AssetClass.Stock),
                    new Tuple<string,AssetClass>("MDLZ", AssetClass.Stock),
                    new Tuple<string,AssetClass>("MTB", AssetClass.Stock),
                    new Tuple<string,AssetClass>("OXY", AssetClass.Stock),
                    new Tuple<string,AssetClass>("PG", AssetClass.Stock),
                    new Tuple<string,AssetClass>("PNC", AssetClass.Stock),
                    new Tuple<string,AssetClass>("PSX", AssetClass.Stock),
                    new Tuple<string,AssetClass>("RH", AssetClass.Stock),
                    new Tuple<string,AssetClass>("QSR", AssetClass.Stock),
                    new Tuple<string,AssetClass>("SIRI", AssetClass.Stock),
                    new Tuple<string,AssetClass>("SPY",AssetClass.ETF),
                    new Tuple<string,AssetClass>("STNE", AssetClass.Stock),
                    new Tuple<string,AssetClass>("STOR", AssetClass.Stock),
                    new Tuple<string,AssetClass>("SU", AssetClass.Stock),
                    new Tuple<string,AssetClass>("SYF", AssetClass.Stock),
                    new Tuple<string,AssetClass>("TEVA", AssetClass.Stock),
                    new Tuple<string,AssetClass>("TRV", AssetClass.Stock),
                    new Tuple<string,AssetClass>("UAL", AssetClass.Stock),
                    new Tuple<string,AssetClass>("UPS", AssetClass.Stock),
                    new Tuple<string,AssetClass>("USB", AssetClass.Stock),
                    new Tuple<string,AssetClass>("V", AssetClass.Stock),
                    new Tuple<string,AssetClass>("VOO", AssetClass.ETF),
                    new Tuple<string,AssetClass>("VRSN", AssetClass.Stock),
                    new Tuple<string,AssetClass>("WFC", AssetClass.Stock),
                });
                
            }

            name = "ETF";
            if (defaultUser.Portfolios.Where(p => p.Name == name).Any() == false)
            {
                context.CreatePortfolio(defaultUser, name, true, new Tuple<string, AssetClass>[] 
                {
                    new Tuple<string,AssetClass>("TQQQ",AssetClass.ETF),
                    new Tuple<string,AssetClass>("SPXL",AssetClass.ETF),
                    new Tuple<string,AssetClass>("TNA",AssetClass.ETF),
                    new Tuple<string,AssetClass>("MIDU",AssetClass.ETF),
                    new Tuple<string,AssetClass>("VNQ",AssetClass.ETF),
                    new Tuple<string,AssetClass>("GEX", AssetClass.ETF),
                });
            }

            name = "Big Technologies";
            if (defaultUser.Portfolios.Where(p => p.Name == name).Any() == false)
            {
                context.CreatePortfolio(defaultUser, name, true, new Tuple<string, AssetClass>[] 
                {
                    new Tuple<string, AssetClass>("AAPL",AssetClass.Stock),
                    new Tuple<string, AssetClass>("AMZN",AssetClass.Stock),
                    new Tuple<string, AssetClass>("CSCO",AssetClass.Stock),
                    new Tuple<string, AssetClass>("GOOGL",AssetClass.Stock),
                    new Tuple<string, AssetClass>("IBM",AssetClass.Stock),
                    new Tuple<string, AssetClass>("MSFT",AssetClass.Stock),
                    new Tuple<string, AssetClass>("ORCL", AssetClass.Stock),
                    new Tuple<string, AssetClass>("AMD", AssetClass.Stock),
                    new Tuple<string, AssetClass>("CRM", AssetClass.Stock),
                    new Tuple<string, AssetClass>("FSLR", AssetClass.Stock),
                    new Tuple<string, AssetClass>("INTC", AssetClass.Stock),
                    new Tuple<string, AssetClass>("MELI", AssetClass.Stock),
                    new Tuple<string, AssetClass>("GLOB", AssetClass.Stock),
                    //new Tuple<string, AssetClass>("", AssetClass.Stock),
                    //new Tuple<string, AssetClass>("", AssetClass.Stock),
                    //new Tuple<string, AssetClass>("", AssetClass.Stock),
                });
            }
        }
    }
}
