using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceManagement.DataAccess.Migrations
{
    public partial class AddInvestmentFundTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvestmentFundTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    InvestmentFundCategoryId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvestmentFundId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentFundTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentFundTransaction_InvestmentFund_InvestmentFundId",
                        column: x => x.InvestmentFundId,
                        principalTable: "InvestmentFund",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvestmentFundTransaction_InvestmentFundCategory_InvestmentFundCategoryId",
                        column: x => x.InvestmentFundCategoryId,
                        principalTable: "InvestmentFundCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentFundTransaction_InvestmentFundCategoryId",
                table: "InvestmentFundTransaction",
                column: "InvestmentFundCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentFundTransaction_InvestmentFundId",
                table: "InvestmentFundTransaction",
                column: "InvestmentFundId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvestmentFundTransaction");
        }
    }
}
