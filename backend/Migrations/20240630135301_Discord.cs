using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class Discord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OriginalSource",
                table: "Thumbnails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginalSource",
                table: "Reels",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginalSource",
                table: "LongVideos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginalSource",
                table: "Images",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginalSource",
                table: "Files",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginalSource",
                table: "Audio",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DiscordChannels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<string>(type: "text", nullable: true),
                    Category = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Topic = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordChannels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscordGuilds",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IconUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordGuilds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscordUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Discriminator = table.Column<string>(type: "text", nullable: true),
                    NickName = table.Column<string>(type: "text", nullable: true),
                    Color = table.Column<string>(type: "text", nullable: true),
                    IsBot = table.Column<bool>(type: "boolean", nullable: false),
                    AvatarUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscordServers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GuildId = table.Column<string>(type: "text", nullable: false),
                    ChannelId = table.Column<string>(type: "text", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "DiscordMessages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TimeStampEdited = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CallEndedTimeStamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsPinned = table.Column<bool>(type: "boolean", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    AuthorId = table.Column<string>(type: "text", nullable: true),
                    DiscordServerId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordMessages_DiscordServers_DiscordServerId",
                        column: x => x.DiscordServerId,
                        principalTable: "DiscordServers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DiscordMessages_DiscordUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "DiscordUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DiscordAttachments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    FileSizeBytes = table.Column<int>(type: "integer", nullable: false),
                    MessageId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordAttachments_DiscordMessages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "DiscordMessages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Embed",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MessageId = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    TimeStamp = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Thumbnail_Url = table.Column<string>(type: "text", nullable: true),
                    Thumbnail_Width = table.Column<int>(type: "integer", nullable: true),
                    Thumbnail_Height = table.Column<int>(type: "integer", nullable: true),
                    EmbedVideo_Url = table.Column<string>(type: "text", nullable: true),
                    EmbedVideo_Width = table.Column<int>(type: "integer", nullable: true),
                    EmbedVideo_Height = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Embed", x => new { x.MessageId, x.Id });
                    table.ForeignKey(
                        name: "FK_Embed_DiscordMessages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "DiscordMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "Reaction",
                columns: table => new
                {
                    MessageId = table.Column<string>(type: "text", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Emoji_Id = table.Column<string>(type: "text", nullable: false),
                    Emoji_Name = table.Column<string>(type: "text", nullable: false),
                    Emoji_Code = table.Column<string>(type: "text", nullable: false),
                    Emoji_IsAnimated = table.Column<bool>(type: "boolean", nullable: false),
                    Emoji_ImageUrl = table.Column<string>(type: "text", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reaction", x => new { x.MessageId, x.Id });
                    table.ForeignKey(
                        name: "FK_Reaction_DiscordMessages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "DiscordMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscordAttachments_MessageId",
                table: "DiscordAttachments",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordMessages_AuthorId",
                table: "DiscordMessages",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordMessages_DiscordServerId",
                table: "DiscordMessages",
                column: "DiscordServerId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordServers_ChannelId",
                table: "DiscordServers",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordServers_GuildId",
                table: "DiscordServers",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageMentions_MessageId",
                table: "MessageMentions",
                column: "MessageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscordAttachments");

            migrationBuilder.DropTable(
                name: "Embed");

            migrationBuilder.DropTable(
                name: "MessageMentions");

            migrationBuilder.DropTable(
                name: "Reaction");

            migrationBuilder.DropTable(
                name: "DiscordMessages");

            migrationBuilder.DropTable(
                name: "DiscordServers");

            migrationBuilder.DropTable(
                name: "DiscordUsers");

            migrationBuilder.DropTable(
                name: "DiscordChannels");

            migrationBuilder.DropTable(
                name: "DiscordGuilds");

            migrationBuilder.DropColumn(
                name: "OriginalSource",
                table: "Thumbnails");

            migrationBuilder.DropColumn(
                name: "OriginalSource",
                table: "Reels");

            migrationBuilder.DropColumn(
                name: "OriginalSource",
                table: "LongVideos");

            migrationBuilder.DropColumn(
                name: "OriginalSource",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "OriginalSource",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "OriginalSource",
                table: "Audio");
        }
    }
}
