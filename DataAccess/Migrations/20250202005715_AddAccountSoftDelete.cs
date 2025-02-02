using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceManagement.DataAccess.Migrations
{
    public partial class AddAccountSoftDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Accounts");
        }
    }
}
