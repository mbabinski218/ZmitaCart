using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZmitaCart.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryOffers");

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "Chats",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "Chats");

            migrationBuilder.CreateTable(
                name: "CategoryOffers",
                columns: table => new
                {
                    OfferId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
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
    }
}
