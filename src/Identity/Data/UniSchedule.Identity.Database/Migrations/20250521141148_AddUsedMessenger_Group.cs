using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniSchedule.Identity.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddUsedMessenger_Group : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsedMessenger",
                table: "Groups",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsedMessenger",
                table: "Groups");
        }
    }
}
