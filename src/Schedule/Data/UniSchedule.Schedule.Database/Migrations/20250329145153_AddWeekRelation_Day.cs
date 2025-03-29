using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniSchedule.Schedule.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddWeekRelation_Day : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Days_Id",
                table: "Classes");

            migrationBuilder.DropTable(
                name: "DayWeek");

            migrationBuilder.AddColumn<Guid>(
                name: "WeekId",
                table: "Days",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Days_WeekId",
                table: "Days",
                column: "WeekId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_DayId",
                table: "Classes",
                column: "DayId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Days_DayId",
                table: "Classes",
                column: "DayId",
                principalTable: "Days",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Days_Weeks_WeekId",
                table: "Days",
                column: "WeekId",
                principalTable: "Weeks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Days_DayId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Days_Weeks_WeekId",
                table: "Days");

            migrationBuilder.DropIndex(
                name: "IX_Days_WeekId",
                table: "Days");

            migrationBuilder.DropIndex(
                name: "IX_Classes_DayId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "WeekId",
                table: "Days");

            migrationBuilder.CreateTable(
                name: "DayWeek",
                columns: table => new
                {
                    DaysId = table.Column<Guid>(type: "uuid", nullable: false),
                    WeekId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayWeek", x => new { x.DaysId, x.WeekId });
                    table.ForeignKey(
                        name: "FK_DayWeek_Days_DaysId",
                        column: x => x.DaysId,
                        principalTable: "Days",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DayWeek_Weeks_WeekId",
                        column: x => x.WeekId,
                        principalTable: "Weeks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DayWeek_WeekId",
                table: "DayWeek",
                column: "WeekId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Days_Id",
                table: "Classes",
                column: "Id",
                principalTable: "Days",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
