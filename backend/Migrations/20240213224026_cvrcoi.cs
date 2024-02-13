using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class cvrcoi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CvrcId",
                table: "Images",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cvrces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cvrces", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_CvrcId",
                table: "Images",
                column: "CvrcId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Cvrces_CvrcId",
                table: "Images",
                column: "CvrcId",
                principalTable: "Cvrces",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Cvrces_CvrcId",
                table: "Images");

            migrationBuilder.DropTable(
                name: "Cvrces");

            migrationBuilder.DropIndex(
                name: "IX_Images_CvrcId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "CvrcId",
                table: "Images");
        }
    }
}
