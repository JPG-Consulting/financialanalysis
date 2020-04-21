using Microsoft.EntityFrameworkCore.Migrations;

namespace FinancialAnalyst.DataAccess.Migrations
{
    public partial class AddAssetType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssetType",
                table: "Transactions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssetType",
                table: "AssetAllocations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssetType",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "AssetType",
                table: "AssetAllocations");
        }
    }
}
