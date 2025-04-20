using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniSchedule.Schedule.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddClassTypeAndLastAcademicWeekNumber_Group : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastAcademicWeekNumber",
                table: "Groups",
                type: "integer",
                nullable: false,
                defaultValue: 16);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Classes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastAcademicWeekNumber",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Classes");
        }
    }
}
