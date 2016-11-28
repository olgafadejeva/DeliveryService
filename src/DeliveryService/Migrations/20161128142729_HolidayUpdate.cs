using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeliveryService.Migrations
{
    public partial class HolidayUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DriverHoliday_Drivers_DriverID",
                table: "DriverHoliday");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DriverHoliday",
                table: "DriverHoliday");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DriverHolidays",
                table: "DriverHoliday",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_DriverHolidays_Drivers_DriverID",
                table: "DriverHoliday",
                column: "DriverID",
                principalTable: "Drivers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.RenameIndex(
                name: "IX_DriverHoliday_DriverID",
                table: "DriverHoliday",
                newName: "IX_DriverHolidays_DriverID");

            migrationBuilder.RenameTable(
                name: "DriverHoliday",
                newName: "DriverHolidays");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DriverHolidays_Drivers_DriverID",
                table: "DriverHolidays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DriverHolidays",
                table: "DriverHolidays");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DriverHoliday",
                table: "DriverHolidays",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_DriverHoliday_Drivers_DriverID",
                table: "DriverHolidays",
                column: "DriverID",
                principalTable: "Drivers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.RenameIndex(
                name: "IX_DriverHolidays_DriverID",
                table: "DriverHolidays",
                newName: "IX_DriverHoliday_DriverID");

            migrationBuilder.RenameTable(
                name: "DriverHolidays",
                newName: "DriverHoliday");
        }
    }
}
