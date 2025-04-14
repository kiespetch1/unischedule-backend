using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniSchedule.Schedule.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddPriority_Announcement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Announcements",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Announcements");
        }
    }
}
