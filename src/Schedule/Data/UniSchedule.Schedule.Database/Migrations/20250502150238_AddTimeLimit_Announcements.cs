using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniSchedule.Schedule.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddTimeLimit_Announcements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AvailableUntil",
                table: "Announcements",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTimeLimited",
                table: "Announcements",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql(@"
                CREATE INDEX idx_announcements_target_gin
                    ON ""Announcements""
                    USING GIN (""Target"");
            ");
        }

        
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableUntil",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "IsTimeLimited",
                table: "Announcements");
        }
    }
}
