using Microsoft.EntityFrameworkCore.Migrations;

namespace FinancialAnalyst.DataAccess.Migrations
{
    public partial class AddAssetAllocations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetAllocation_Portfolios_PortfolioId",
                table: "AssetAllocation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssetAllocation",
                table: "AssetAllocation");

            migrationBuilder.RenameTable(
                name: "AssetAllocation",
                newName: "AssetAllocations");

            migrationBuilder.RenameIndex(
                name: "IX_AssetAllocation_PortfolioId",
                table: "AssetAllocations",
                newName: "IX_AssetAllocations_PortfolioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssetAllocations",
                table: "AssetAllocations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetAllocations_Portfolios_PortfolioId",
                table: "AssetAllocations",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetAllocations_Portfolios_PortfolioId",
                table: "AssetAllocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssetAllocations",
                table: "AssetAllocations");

            migrationBuilder.RenameTable(
                name: "AssetAllocations",
                newName: "AssetAllocation");

            migrationBuilder.RenameIndex(
                name: "IX_AssetAllocations_PortfolioId",
                table: "AssetAllocation",
                newName: "IX_AssetAllocation_PortfolioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssetAllocation",
                table: "AssetAllocation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetAllocation_Portfolios_PortfolioId",
                table: "AssetAllocation",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
