using Microsoft.EntityFrameworkCore.Migrations;

namespace FinancialAnalyst.DataAccess.Migrations
{
    public partial class AddCalculationsToPortfolio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalCash",
                table: "Portfolios");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "AssetAllocations");

            migrationBuilder.AlterColumn<decimal>(
                name: "InitialBalance",
                table: "Portfolios",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<decimal>(
                name: "Cash",
                table: "Portfolios",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSimulated",
                table: "Portfolios",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "MarketValue",
                table: "Portfolios",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCosts",
                table: "Portfolios",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Costs",
                table: "AssetAllocations",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MarketValue",
                table: "AssetAllocations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "AssetAllocations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cash",
                table: "Portfolios");

            migrationBuilder.DropColumn(
                name: "IsSimulated",
                table: "Portfolios");

            migrationBuilder.DropColumn(
                name: "MarketValue",
                table: "Portfolios");

            migrationBuilder.DropColumn(
                name: "TotalCosts",
                table: "Portfolios");

            migrationBuilder.DropColumn(
                name: "Costs",
                table: "AssetAllocations");

            migrationBuilder.DropColumn(
                name: "MarketValue",
                table: "AssetAllocations");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "AssetAllocations");

            migrationBuilder.AlterColumn<decimal>(
                name: "InitialBalance",
                table: "Portfolios",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCash",
                table: "Portfolios",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "AssetAllocations",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
