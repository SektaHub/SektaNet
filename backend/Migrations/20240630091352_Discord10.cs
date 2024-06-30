using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class Discord10 : Migration
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
                name: "Descriminator",
                table: "DiscordUsers");

            migrationBuilder.RenameColumn(
                name: "MessageId",
                table: "DiscordUsers",
                newName: "Discriminator");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "DiscordMessages",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "MessageMentions",
                columns: table => new
                {
                    MentioningMessagesId = table.Column<string>(type: "text", nullable: false),
                    MentionsId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageMentions", x => new { x.MentioningMessagesId, x.MentionsId });
                    table.ForeignKey(
                        name: "FK_MessageMentions_DiscordMessages_MentioningMessagesId",
                        column: x => x.MentioningMessagesId,
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
                name: "IX_MessageMentions_MentionsId",
                table: "MessageMentions",
                column: "MentionsId");

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

            migrationBuilder.RenameColumn(
                name: "Discriminator",
                table: "DiscordUsers",
                newName: "MessageId");

            migrationBuilder.AddColumn<string>(
                name: "Descriminator",
                table: "DiscordUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "DiscordMessages",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordUsers_MessageId",
                table: "DiscordUsers",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordMessages_DiscordUsers_AuthorId",
                table: "DiscordMessages",
                column: "AuthorId",
                principalTable: "DiscordUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordUsers_DiscordMessages_MessageId",
                table: "DiscordUsers",
                column: "MessageId",
                principalTable: "DiscordMessages",
                principalColumn: "Id");
        }
    }
}
