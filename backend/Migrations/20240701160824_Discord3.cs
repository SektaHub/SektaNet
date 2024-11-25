using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class Discord3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscordMessages_DiscordServers_DiscordServerId",
                table: "DiscordMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_Embed_DiscordMessages_MessageId",
                table: "Embed");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Embed",
                table: "Embed");

            migrationBuilder.AlterColumn<string>(
                name: "MessageId",
                table: "Embed",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<Guid>(
                name: "DiscordServerId",
                table: "DiscordMessages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Embed",
                table: "Embed",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Embed_MessageId",
                table: "Embed",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordMessages_DiscordServers_DiscordServerId",
                table: "DiscordMessages",
                column: "DiscordServerId",
                principalTable: "DiscordServers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Embed_DiscordMessages_MessageId",
                table: "Embed",
                column: "MessageId",
                principalTable: "DiscordMessages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscordMessages_DiscordServers_DiscordServerId",
                table: "DiscordMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_Embed_DiscordMessages_MessageId",
                table: "Embed");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Embed",
                table: "Embed");

            migrationBuilder.DropIndex(
                name: "IX_Embed_MessageId",
                table: "Embed");

            migrationBuilder.AlterColumn<string>(
                name: "MessageId",
                table: "Embed",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DiscordServerId",
                table: "DiscordMessages",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Embed",
                table: "Embed",
                columns: new[] { "MessageId", "Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordMessages_DiscordServers_DiscordServerId",
                table: "DiscordMessages",
                column: "DiscordServerId",
                principalTable: "DiscordServers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Embed_DiscordMessages_MessageId",
                table: "Embed",
                column: "MessageId",
                principalTable: "DiscordMessages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
