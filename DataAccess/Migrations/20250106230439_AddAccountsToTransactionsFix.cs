using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceManagement.DataAccess.Migrations
{
    public partial class AddAccountsToTransactionsFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "AccountIdentifier",
                table: "FinancialTransactions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Identifier",
                table: "Accounts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Accounts_Identifier",
                table: "Accounts",
                column: "Identifier");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransactions_AccountIdentifier",
                table: "FinancialTransactions",
                column: "AccountIdentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialTransactions_Accounts_AccountIdentifier",
                table: "FinancialTransactions",
                column: "AccountIdentifier",
                principalTable: "Accounts",
                principalColumn: "Identifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialTransactions_Accounts_AccountIdentifier",
                table: "FinancialTransactions");

            migrationBuilder.DropIndex(
                name: "IX_FinancialTransactions_AccountIdentifier",
                table: "FinancialTransactions");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Accounts_Identifier",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "AccountIdentifier",
                table: "FinancialTransactions");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "FinancialTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Identifier",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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
    }
}
