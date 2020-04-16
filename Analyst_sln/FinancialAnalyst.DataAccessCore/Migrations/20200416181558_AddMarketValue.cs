using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FinancialAnalyst.DataAccess.Migrations
{
    public partial class AddMarketValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarketValue",
                table: "AssetAllocations");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "AssetAllocations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PriceDate",
                table: "AssetAllocations",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TheoricalPrice",
                table: "AssetAllocations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TheoricalPriceDate",
                table: "AssetAllocations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "AssetAllocations");

            migrationBuilder.DropColumn(
                name: "PriceDate",
                table: "AssetAllocations");

            migrationBuilder.DropColumn(
                name: "TheoricalPrice",
                table: "AssetAllocations");

            migrationBuilder.DropColumn(
                name: "TheoricalPriceDate",
                table: "AssetAllocations");

            migrationBuilder.AddColumn<decimal>(
                name: "MarketValue",
                table: "AssetAllocations",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
