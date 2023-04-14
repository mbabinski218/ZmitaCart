using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZmitaCart.Infrastructure.Persistence.Migrations
{
    public partial class roleFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role_Code",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role_Code",
                table: "Users");
        }
    }
}
