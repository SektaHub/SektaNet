using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class Discord9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Embed_DiscordUsers_AuthorId",
                table: "Embed");

            migrationBuilder.DropIndex(
                name: "IX_Embed_AuthorId",
                table: "Embed");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Embed");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Embed",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Embed_AuthorId",
                table: "Embed",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Embed_DiscordUsers_AuthorId",
                table: "Embed",
                column: "AuthorId",
                principalTable: "DiscordUsers",
                principalColumn: "Id");
        }
    }
}
