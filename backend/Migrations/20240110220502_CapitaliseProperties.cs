using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class CapitaliseProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "fileExtension",
                table: "Reels",
                newName: "FileExtension");

            migrationBuilder.RenameColumn(
                name: "generatedCaption",
                table: "Images",
                newName: "GeneratedCaption");

            migrationBuilder.RenameColumn(
                name: "fileExtension",
                table: "Images",
                newName: "FileExtension");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileExtension",
                table: "Reels",
                newName: "fileExtension");

            migrationBuilder.RenameColumn(
                name: "GeneratedCaption",
                table: "Images",
                newName: "generatedCaption");

            migrationBuilder.RenameColumn(
                name: "FileExtension",
                table: "Images",
                newName: "fileExtension");
        }
    }
}
