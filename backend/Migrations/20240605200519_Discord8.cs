using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class Discord8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reaction_DiscordEmojis_EmojiId",
                table: "Reaction");

            migrationBuilder.DropTable(
                name: "DiscordEmojis");

            migrationBuilder.DropIndex(
                name: "IX_Reaction_EmojiId",
                table: "Reaction");

            migrationBuilder.RenameColumn(
                name: "EmojiId",
                table: "Reaction",
                newName: "Emoji_Name");

            migrationBuilder.AddColumn<string>(
                name: "Emoji_Code",
                table: "Reaction",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Emoji_Id",
                table: "Reaction",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Emoji_ImageUrl",
                table: "Reaction",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Emoji_IsAnimated",
                table: "Reaction",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Emoji_Code",
                table: "Reaction");

            migrationBuilder.DropColumn(
                name: "Emoji_Id",
                table: "Reaction");

            migrationBuilder.DropColumn(
                name: "Emoji_ImageUrl",
                table: "Reaction");

            migrationBuilder.DropColumn(
                name: "Emoji_IsAnimated",
                table: "Reaction");

            migrationBuilder.RenameColumn(
                name: "Emoji_Name",
                table: "Reaction",
                newName: "EmojiId");

            migrationBuilder.CreateTable(
                name: "DiscordEmojis",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    IsAnimated = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordEmojis", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reaction_EmojiId",
                table: "Reaction",
                column: "EmojiId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reaction_DiscordEmojis_EmojiId",
                table: "Reaction",
                column: "EmojiId",
                principalTable: "DiscordEmojis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
