using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class Discord12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscordMessages_DiscordUsers_AuthorId",
                table: "DiscordMessages");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "DiscordMessages",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordMessages_DiscordUsers_AuthorId",
                table: "DiscordMessages",
                column: "AuthorId",
                principalTable: "DiscordUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscordMessages_DiscordUsers_AuthorId",
                table: "DiscordMessages");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "DiscordMessages",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordMessages_DiscordUsers_AuthorId",
                table: "DiscordMessages",
                column: "AuthorId",
                principalTable: "DiscordUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
