using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class DiscordExportRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscordMessages_DiscordServers_DiscordServerId",
                table: "DiscordMessages");

            migrationBuilder.DropTable(
                name: "DiscordServers");

            migrationBuilder.CreateTable(
                name: "DiscordChannelExports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GuildId = table.Column<string>(type: "text", nullable: false),
                    ChannelId = table.Column<string>(type: "text", nullable: false),
                    ExportedAt = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordChannelExports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordChannelExports_DiscordChannels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "DiscordChannels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscordChannelExports_DiscordGuilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "DiscordGuilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscordChannelExports_ChannelId",
                table: "DiscordChannelExports",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordChannelExports_GuildId",
                table: "DiscordChannelExports",
                column: "GuildId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordMessages_DiscordChannelExports_DiscordServerId",
                table: "DiscordMessages",
                column: "DiscordServerId",
                principalTable: "DiscordChannelExports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscordMessages_DiscordChannelExports_DiscordServerId",
                table: "DiscordMessages");

            migrationBuilder.DropTable(
                name: "DiscordChannelExports");

            migrationBuilder.CreateTable(
                name: "DiscordServers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChannelId = table.Column<string>(type: "text", nullable: false),
                    GuildId = table.Column<string>(type: "text", nullable: false),
                    ExportedAt = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordServers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordServers_DiscordChannels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "DiscordChannels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscordServers_DiscordGuilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "DiscordGuilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscordServers_ChannelId",
                table: "DiscordServers",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordServers_GuildId",
                table: "DiscordServers",
                column: "GuildId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordMessages_DiscordServers_DiscordServerId",
                table: "DiscordMessages",
                column: "DiscordServerId",
                principalTable: "DiscordServers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
