using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniSchedule.Schedule.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveWeekType_Group : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeekType",
                table: "Groups");

            migrationBuilder.AlterColumn<string>(
                name: "Link",
                table: "Locations",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Link",
                table: "Locations",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WeekType",
                table: "Groups",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
