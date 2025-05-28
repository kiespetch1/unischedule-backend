using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniSchedule.Bot.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddConversationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConversationName",
                table: "GroupMessengerConversation",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConversationName",
                table: "GroupMessengerConversation");
        }
    }
}
