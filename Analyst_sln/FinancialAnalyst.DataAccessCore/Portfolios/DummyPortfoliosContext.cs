using FinancialAnalyst.Common.Entities.Portfolios;
using FinancialAnalyst.Common.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System;

namespace FinancialAnalyst.DataAccess.Portfolios
{
    public class DummyPortfoliosContext : PortfoliosContext
    {
        /*
         * https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet#using-the-tools
         * 
         * Other target frameworks
         * 
         * The CLI tools work with .NET Core projects and .NET Framework projects. 
         * Apps that have the EF Core model in a .NET Standard class library might not have a .NET Core or .NET Framework project. 
         * For example, this is true of Xamarin and Universal Windows Platform apps. 
         * In such cases, you can create a .NET Core console app project whose only purpose is to act as startup project for the tools. 
         * The project can be a dummy project with no real code — it is only needed to provide a target for the tooling.
         * 
         * Why is a dummy project required? 
         * As mentioned earlier, the tools have to execute application code at design time. 
         * To do that, they need to use the .NET Core runtime. 
         * When the EF Core model is in a project that targets .NET Core or .NET Framework, the EF Core tools borrow the runtime from the project. 
         * They can't do that if the EF Core model is in a .NET Standard class library. 
         * The .NET Standard is not an actual .NET implementation; it's a specification of a set of APIs that .NET implementations must support. 
         * Therefore .NET Standard is not sufficient for the EF Core tools to execute application code. 
         * The dummy project you create to use as startup project provides a concrete target platform into which the tools can load the .NET Standard class library.
         * 
         * **************************************************************************************************************************************
         * 
         * To add a new table, execute command in Package Manager Console:
         * dotnet ef migrations add <NameOfMigration> --project FinancialAnalyst.DataAccessCore
         * Then, copy migration from DataAccessCore to DataAccess (.Net Standard)
         * 
         */
    }
}
