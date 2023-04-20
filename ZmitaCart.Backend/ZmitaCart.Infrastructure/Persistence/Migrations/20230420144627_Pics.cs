using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZmitaCart.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Pics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureName",
                table: "OfferPictures");

            migrationBuilder.RenameColumn(
                name: "PictureUrl",
                table: "OfferPictures",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "OfferPictures",
                newName: "PictureUrl");

            migrationBuilder.AddColumn<string>(
                name: "PictureName",
                table: "OfferPictures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
