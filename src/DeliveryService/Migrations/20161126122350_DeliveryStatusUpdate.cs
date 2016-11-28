using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeliveryService.Migrations
{
    public partial class DeliveryStatusUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryStatus_Drivers_AssignedToId",
                table: "DeliveryStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Companies_CompanyID",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_CompanyID",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryStatus_AssignedToId",
                table: "DeliveryStatus");

            migrationBuilder.DropColumn(
                name: "CompanyID",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "AssignedToId",
                table: "DeliveryStatus");

            migrationBuilder.DropColumn(
                name: "AddedToRoute",
                table: "Deliveries");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveredDate",
                table: "DeliveryStatus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveredDate",
                table: "DeliveryStatus");

            migrationBuilder.AddColumn<int>(
                name: "CompanyID",
                table: "Drivers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssignedToId",
                table: "DeliveryStatus",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AddedToRoute",
                table: "Deliveries",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_CompanyID",
                table: "Drivers",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryStatus_AssignedToId",
                table: "DeliveryStatus",
                column: "AssignedToId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryStatus_Drivers_AssignedToId",
                table: "DeliveryStatus",
                column: "AssignedToId",
                principalTable: "Drivers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Companies_CompanyID",
                table: "Drivers",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
