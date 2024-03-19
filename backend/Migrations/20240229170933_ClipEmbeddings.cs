using Microsoft.EntityFrameworkCore.Migrations;
using Pgvector;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class ClipEmbeddings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Images_CaptionEmbedding",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "CaptionEmbedding",
                table: "Images");

            migrationBuilder.AddColumn<Vector>(
                name: "ClipEmbedding",
                table: "Images",
                type: "vector(768)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_ClipEmbedding",
                table: "Images",
                column: "ClipEmbedding")
                .Annotation("Npgsql:IndexMethod", "ivfflat")
                .Annotation("Npgsql:IndexOperators", new[] { "vector_l2_ops" })
                .Annotation("Npgsql:StorageParameter:lists", 100);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Images_ClipEmbedding",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ClipEmbedding",
                table: "Images");

            migrationBuilder.AddColumn<Vector>(
                name: "CaptionEmbedding",
                table: "Images",
                type: "vector(384)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_CaptionEmbedding",
                table: "Images",
                column: "CaptionEmbedding")
                .Annotation("Npgsql:IndexMethod", "ivfflat")
                .Annotation("Npgsql:IndexOperators", new[] { "vector_l2_ops" })
                .Annotation("Npgsql:StorageParameter:lists", 100);
        }
    }
}
