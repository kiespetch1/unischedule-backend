using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniSchedule.Schedule.Database.Migrations
{
    /// <inheritdoc />
    public partial class FixTypeNaming_Week : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WeekType",
                table: "Weeks",
                newName: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Weeks",
                newName: "WeekType");
        }
    }
}
