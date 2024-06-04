using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class Discord6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscordRoles_DiscordUsers_DiscordUserId",
                table: "DiscordRoles");

            migrationBuilder.DropIndex(
                name: "IX_DiscordRoles_DiscordUserId",
                table: "DiscordRoles");

            migrationBuilder.DropColumn(
                name: "DiscordUserId",
                table: "DiscordRoles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiscordUserId",
                table: "DiscordRoles",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscordRoles_DiscordUserId",
                table: "DiscordRoles",
                column: "DiscordUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordRoles_DiscordUsers_DiscordUserId",
                table: "DiscordRoles",
                column: "DiscordUserId",
                principalTable: "DiscordUsers",
                principalColumn: "Id");
        }
    }
}
