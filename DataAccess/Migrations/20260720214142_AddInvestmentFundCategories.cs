using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceManagement.DataAccess.Migrations
{
    public partial class AddInvestmentFundCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvestmentFundCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    InvestmentFundId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentFundCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentFundCategory_InvestmentFund_InvestmentFundId",
                        column: x => x.InvestmentFundId,
                        principalTable: "InvestmentFund",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentFundCategory_InvestmentFundId",
                table: "InvestmentFundCategory",
                column: "InvestmentFundId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvestmentFundCategory");
        }
    }
}
