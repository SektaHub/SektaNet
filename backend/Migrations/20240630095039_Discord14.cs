﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class Discord14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscordMessages_DiscordUsers_AuthorId",
                table: "DiscordMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscordUsers_DiscordMessages_MessageId",
                table: "DiscordUsers");

            migrationBuilder.DropIndex(
                name: "IX_DiscordUsers_MessageId",
                table: "DiscordUsers");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "DiscordUsers");

            migrationBuilder.CreateTable(
                name: "MessageMentions",
                columns: table => new
                {
                    MentionsId = table.Column<string>(type: "text", nullable: false),
                    MessageId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageMentions", x => new { x.MentionsId, x.MessageId });
                    table.ForeignKey(
                        name: "FK_MessageMentions_DiscordMessages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "DiscordMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageMentions_DiscordUsers_MentionsId",
                        column: x => x.MentionsId,
                        principalTable: "DiscordUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageMentions_MessageId",
                table: "MessageMentions",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordMessages_DiscordUsers_AuthorId",
                table: "DiscordMessages",
                column: "AuthorId",
                principalTable: "DiscordUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscordMessages_DiscordUsers_AuthorId",
                table: "DiscordMessages");

            migrationBuilder.DropTable(
                name: "MessageMentions");

            migrationBuilder.AddColumn<string>(
                name: "MessageId",
                table: "DiscordUsers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscordUsers_MessageId",
                table: "DiscordUsers",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordMessages_DiscordUsers_AuthorId",
                table: "DiscordMessages",
                column: "AuthorId",
                principalTable: "DiscordUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordUsers_DiscordMessages_MessageId",
                table: "DiscordUsers",
                column: "MessageId",
                principalTable: "DiscordMessages",
                principalColumn: "Id");
        }
    }
}