using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class FileSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Thumbnails");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Reels");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "LongVideos");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Audio");

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "Thumbnails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "Reels",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "LongVideos",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "Images",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "Files",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "Audio",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "Thumbnails");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "Reels");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "LongVideos");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "Audio");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Thumbnails",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Reels",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "LongVideos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Images",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Files",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Audio",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
