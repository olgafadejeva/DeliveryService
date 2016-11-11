using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeliveryService.Migrations
{
    public partial class Routes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Route_RouteID",
                table: "Deliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_Route_Companies_CompanyID",
                table: "Route");

            migrationBuilder.DropForeignKey(
                name: "FK_Route_Drivers_DriverID",
                table: "Route");

            migrationBuilder.DropForeignKey(
                name: "FK_Route_Addresses_PickUpAddressID",
                table: "Route");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Route",
                table: "Route");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Routes",
                table: "Route",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Routes_RouteID",
                table: "Deliveries",
                column: "RouteID",
                principalTable: "Route",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Companies_CompanyID",
                table: "Route",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Drivers_DriverID",
                table: "Route",
                column: "DriverID",
                principalTable: "Drivers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Addresses_PickUpAddressID",
                table: "Route",
                column: "PickUpAddressID",
                principalTable: "Addresses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.RenameIndex(
                name: "IX_Route_PickUpAddressID",
                table: "Route",
                newName: "IX_Routes_PickUpAddressID");

            migrationBuilder.RenameIndex(
                name: "IX_Route_DriverID",
                table: "Route",
                newName: "IX_Routes_DriverID");

            migrationBuilder.RenameIndex(
                name: "IX_Route_CompanyID",
                table: "Route",
                newName: "IX_Routes_CompanyID");

            migrationBuilder.RenameTable(
                name: "Route",
                newName: "Routes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Routes_RouteID",
                table: "Deliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Companies_CompanyID",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Drivers_DriverID",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Addresses_PickUpAddressID",
                table: "Routes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Routes",
                table: "Routes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Route",
                table: "Routes",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Route_RouteID",
                table: "Deliveries",
                column: "RouteID",
                principalTable: "Routes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Route_Companies_CompanyID",
                table: "Routes",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Route_Drivers_DriverID",
                table: "Routes",
                column: "DriverID",
                principalTable: "Drivers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Route_Addresses_PickUpAddressID",
                table: "Routes",
                column: "PickUpAddressID",
                principalTable: "Addresses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.RenameIndex(
                name: "IX_Routes_PickUpAddressID",
                table: "Routes",
                newName: "IX_Route_PickUpAddressID");

            migrationBuilder.RenameIndex(
                name: "IX_Routes_DriverID",
                table: "Routes",
                newName: "IX_Route_DriverID");

            migrationBuilder.RenameIndex(
                name: "IX_Routes_CompanyID",
                table: "Routes",
                newName: "IX_Route_CompanyID");

            migrationBuilder.RenameTable(
                name: "Routes",
                newName: "Route");
        }
    }
}
