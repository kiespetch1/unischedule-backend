using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniSchedule.Schedule.Database.Migrations
{
    /// <inheritdoc />
    public partial class FixRelation_Class : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Locations_Id",
                table: "Classes");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_LocationId",
                table: "Classes",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Locations_LocationId",
                table: "Classes",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Locations_LocationId",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_LocationId",
                table: "Classes");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Locations_Id",
                table: "Classes",
                column: "Id",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
