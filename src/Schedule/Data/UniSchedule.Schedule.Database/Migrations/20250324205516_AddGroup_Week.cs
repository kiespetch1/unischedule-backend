using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniSchedule.Schedule.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddGroup_Week : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "Weeks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Weeks_GroupId",
                table: "Weeks",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Weeks_Groups_GroupId",
                table: "Weeks",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weeks_Groups_GroupId",
                table: "Weeks");

            migrationBuilder.DropIndex(
                name: "IX_Weeks_GroupId",
                table: "Weeks");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Weeks");
        }
    }
}
