using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceManagement.DataAccess.Migrations
{
    public partial class AddAccountsToTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "FinancialTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransactions_AccountId",
                table: "FinancialTransactions",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialTransactions_Accounts_AccountId",
                table: "FinancialTransactions",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialTransactions_Accounts_AccountId",
                table: "FinancialTransactions");

            migrationBuilder.DropIndex(
                name: "IX_FinancialTransactions_AccountId",
                table: "FinancialTransactions");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "FinancialTransactions");
        }
    }
}
