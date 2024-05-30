using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AuthorizedRoles2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isPrivate",
                table: "Thumbnails");

            migrationBuilder.DropColumn(
                name: "isPrivate",
                table: "Reels");

            migrationBuilder.DropColumn(
                name: "isPrivate",
                table: "LongVideos");

            migrationBuilder.DropColumn(
                name: "isPrivate",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "isPrivate",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "isPrivate",
                table: "Audio");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isPrivate",
                table: "Thumbnails",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isPrivate",
                table: "Reels",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isPrivate",
                table: "LongVideos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isPrivate",
                table: "Images",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isPrivate",
                table: "Files",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isPrivate",
                table: "Audio",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
