using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZmitaCart.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PicsFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfferPictures_Offers_OfferId",
                table: "OfferPictures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfferPictures",
                table: "OfferPictures");

            migrationBuilder.RenameTable(
                name: "OfferPictures",
                newName: "Pictures");

            migrationBuilder.RenameIndex(
                name: "IX_OfferPictures_OfferId",
                table: "Pictures",
                newName: "IX_Pictures_OfferId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pictures",
                table: "Pictures",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Offers_OfferId",
                table: "Pictures",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Offers_OfferId",
                table: "Pictures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pictures",
                table: "Pictures");

            migrationBuilder.RenameTable(
                name: "Pictures",
                newName: "OfferPictures");

            migrationBuilder.RenameIndex(
                name: "IX_Pictures_OfferId",
                table: "OfferPictures",
                newName: "IX_OfferPictures_OfferId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfferPictures",
                table: "OfferPictures",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OfferPictures_Offers_OfferId",
                table: "OfferPictures",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
