using Microsoft.EntityFrameworkCore.Migrations;

namespace FinancialAnalyst.DataAccess.Migrations
{
    public partial class AddCashflowType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CashflowType",
                table: "Transactions",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CashflowType",
                table: "Transactions");
        }
    }
}
