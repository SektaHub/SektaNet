using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class Thumbnails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Duration",
                table: "Reels",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailId",
                table: "Reels",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Duration",
                table: "LongVideos",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailId",
                table: "LongVideos",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Thumbnails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    FileExtension = table.Column<string>(type: "text", nullable: false),
                    Tags = table.Column<string>(type: "text", nullable: true),
                    DateUploaded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isPrivate = table.Column<bool>(type: "boolean", nullable: false),
                    OwnerId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Thumbnails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Thumbnails_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reels_ThumbnailId",
                table: "Reels",
                column: "ThumbnailId");

            migrationBuilder.CreateIndex(
                name: "IX_LongVideos_ThumbnailId",
                table: "LongVideos",
                column: "ThumbnailId");

            migrationBuilder.CreateIndex(
                name: "IX_Thumbnails_OwnerId",
                table: "Thumbnails",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_LongVideos_Thumbnails_ThumbnailId",
                table: "LongVideos",
                column: "ThumbnailId",
                principalTable: "Thumbnails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reels_Thumbnails_ThumbnailId",
                table: "Reels",
                column: "ThumbnailId",
                principalTable: "Thumbnails",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LongVideos_Thumbnails_ThumbnailId",
                table: "LongVideos");

            migrationBuilder.DropForeignKey(
                name: "FK_Reels_Thumbnails_ThumbnailId",
                table: "Reels");

            migrationBuilder.DropTable(
                name: "Thumbnails");

            migrationBuilder.DropIndex(
                name: "IX_Reels_ThumbnailId",
                table: "Reels");

            migrationBuilder.DropIndex(
                name: "IX_LongVideos_ThumbnailId",
                table: "LongVideos");

            migrationBuilder.DropColumn(
                name: "ThumbnailId",
                table: "Reels");

            migrationBuilder.DropColumn(
                name: "ThumbnailId",
                table: "LongVideos");

            migrationBuilder.AlterColumn<int>(
                name: "Duration",
                table: "Reels",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "Duration",
                table: "LongVideos",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
