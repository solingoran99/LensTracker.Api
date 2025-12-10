using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LensTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddEndDateAndStopFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "LensSessions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "LensSessions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "StoppedEarlyDate",
                table: "LensSessions",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "LensSessions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "LensSessions");

            migrationBuilder.DropColumn(
                name: "StoppedEarlyDate",
                table: "LensSessions");
        }
    }
}
