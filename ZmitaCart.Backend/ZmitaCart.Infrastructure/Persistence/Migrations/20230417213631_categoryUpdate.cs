using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZmitaCart.Infrastructure.Persistence.Migrations
{
    public partial class categoryUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryOffers",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    OfferId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryOffers", x => new { x.OfferId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_CategoryOffers_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CategoryOffers_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryOffers_CategoryId",
                table: "CategoryOffers",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryOffers");
        }
    }
}
