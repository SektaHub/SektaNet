using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AuthorizedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<string>>(
                name: "AuthorizedRoles",
                table: "Thumbnails",
                type: "text[]",
                nullable: false);

            migrationBuilder.AddColumn<List<string>>(
                name: "AuthorizedRoles",
                table: "Reels",
                type: "text[]",
                nullable: false);

            migrationBuilder.AddColumn<List<string>>(
                name: "AuthorizedRoles",
                table: "LongVideos",
                type: "text[]",
                nullable: false);

            migrationBuilder.AddColumn<List<string>>(
                name: "AuthorizedRoles",
                table: "Images",
                type: "text[]",
                nullable: false);

            migrationBuilder.AddColumn<List<string>>(
                name: "AuthorizedRoles",
                table: "Files",
                type: "text[]",
                nullable: false);

            migrationBuilder.AddColumn<List<string>>(
                name: "AuthorizedRoles",
                table: "Audio",
                type: "text[]",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorizedRoles",
                table: "Thumbnails");

            migrationBuilder.DropColumn(
                name: "AuthorizedRoles",
                table: "Reels");

            migrationBuilder.DropColumn(
                name: "AuthorizedRoles",
                table: "LongVideos");

            migrationBuilder.DropColumn(
                name: "AuthorizedRoles",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "AuthorizedRoles",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "AuthorizedRoles",
                table: "Audio");
        }
    }
}
