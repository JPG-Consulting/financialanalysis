using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FinancialAnalyst.DataAccess.Migrations
{
    public partial class RefactorAssets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssetType",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "AssetType",
                table: "AssetAllocations");

            migrationBuilder.DropColumn(
                name: "Exchange",
                table: "AssetAllocations");

            migrationBuilder.DropColumn(
                name: "Ticker",
                table: "AssetAllocations");

            migrationBuilder.AlterColumn<string>(
                name: "Symbol",
                table: "Transactions",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "RegFee",
                table: "Transactions",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "NetCashBalance",
                table: "Transactions",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Transactions",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssetId",
                table: "Transactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Portfolios",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssetId",
                table: "AssetAllocations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ticker = table.Column<string>(nullable: false),
                    Exchange = table.Column<int>(nullable: true),
                    AssetClass = table.Column<int>(nullable: false),
                    LastPrice = table.Column<decimal>(nullable: true),
                    LastPrice_Date = table.Column<DateTime>(nullable: true),
                    UnderlyingAssetId = table.Column<int>(nullable: true),
                    Change = table.Column<double>(nullable: true),
                    Bid = table.Column<double>(nullable: true),
                    Ask = table.Column<double>(nullable: true),
                    Volume = table.Column<int>(nullable: true),
                    OpenInterest = table.Column<int>(nullable: true),
                    Strike = table.Column<double>(nullable: true),
                    ExpirationDate = table.Column<DateTime>(nullable: true),
                    TheoricalValue = table.Column<double>(nullable: true),
                    OptionClass = table.Column<int>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    WebSite = table.Column<string>(nullable: true),
                    Volatility = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assets_Assets_UnderlyingAssetId",
                        column: x => x.UnderlyingAssetId,
                        principalTable: "Assets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HistoricalPrices",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Close = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Volume = table.Column<decimal>(nullable: false),
                    AssetId = table.Column<int>(nullable: false),
                    Open = table.Column<decimal>(nullable: true),
                    High = table.Column<decimal>(nullable: true),
                    Low = table.Column<decimal>(nullable: true),
                    AdjustedClose = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricalPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricalPrices_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AssetId",
                table: "Transactions",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetAllocations_AssetId",
                table: "AssetAllocations",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_Ticker",
                table: "Assets",
                column: "Ticker",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assets_UnderlyingAssetId",
                table: "Assets",
                column: "UnderlyingAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricalPrices_AssetId",
                table: "HistoricalPrices",
                column: "AssetId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetAllocations_Assets_AssetId",
                table: "AssetAllocations",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Assets_AssetId",
                table: "Transactions",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetAllocations_Assets_AssetId",
                table: "AssetAllocations");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Assets_AssetId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "HistoricalPrices");

            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_AssetId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_AssetAllocations_AssetId",
                table: "AssetAllocations");

            migrationBuilder.DropColumn(
                name: "AssetId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "AssetId",
                table: "AssetAllocations");

            migrationBuilder.AlterColumn<string>(
                name: "Symbol",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<decimal>(
                name: "RegFee",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "NetCashBalance",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "AssetType",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Portfolios",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "AssetType",
                table: "AssetAllocations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Exchange",
                table: "AssetAllocations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ticker",
                table: "AssetAllocations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
