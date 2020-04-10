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
            if(context.Users.Where(u => u.UserName == "sgastia").Any() == false)
            {
                context.Users.Add(new Common.Entities.Users.User()
                {
                    UserName = "sgastia",
                    Email = "sgastia@gmail.com",
                });
                context.SaveChanges();
            }
        }
    }
}
