using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniSchedule.Schedule.Database.Migrations
{
    /// <inheritdoc />
    public partial class MakeTargetRequired_Announcement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                 UPDATE ""Announcements""
                 SET ""Target"" = '{}'::jsonb
                 WHERE ""Target"" IS NULL;
            ");
            
            migrationBuilder.AlterColumn<string>(
                name: "Target",
                table: "Announcements",
                type: "jsonb",
                nullable: false,
                defaultValue: "{}",
                oldClrType: typeof(string),
                oldType: "jsonb",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Target",
                table: "Announcements",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "jsonb");
        }
    }
}
