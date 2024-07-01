using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class Discord4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Embed_DiscordMessages_MessageId",
                table: "Embed");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Embed",
                table: "Embed");

            migrationBuilder.RenameTable(
                name: "Embed",
                newName: "DiscordEmbeds");

            migrationBuilder.RenameIndex(
                name: "IX_Embed_MessageId",
                table: "DiscordEmbeds",
                newName: "IX_DiscordEmbeds_MessageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiscordEmbeds",
                table: "DiscordEmbeds",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordEmbeds_DiscordMessages_MessageId",
                table: "DiscordEmbeds",
                column: "MessageId",
                principalTable: "DiscordMessages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscordEmbeds_DiscordMessages_MessageId",
                table: "DiscordEmbeds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiscordEmbeds",
                table: "DiscordEmbeds");

            migrationBuilder.RenameTable(
                name: "DiscordEmbeds",
                newName: "Embed");

            migrationBuilder.RenameIndex(
                name: "IX_DiscordEmbeds_MessageId",
                table: "Embed",
                newName: "IX_Embed_MessageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Embed",
                table: "Embed",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Embed_DiscordMessages_MessageId",
                table: "Embed",
                column: "MessageId",
                principalTable: "DiscordMessages",
                principalColumn: "Id");
        }
    }
}
