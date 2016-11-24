﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeliveryService.Migrations
{
    public partial class UpdateRouteAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Drivers_AssignedToID",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_AssignedToID",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "AssignedToID",
                table: "Routes");

            migrationBuilder.AddColumn<int>(
                name: "DriverID",
                table: "Routes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Routes_DriverID",
                table: "Routes",
                column: "DriverID");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Drivers_DriverID",
                table: "Routes",
                column: "DriverID",
                principalTable: "Drivers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Drivers_DriverID",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_DriverID",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "DriverID",
                table: "Routes");

            migrationBuilder.AddColumn<int>(
                name: "AssignedToID",
                table: "Routes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Routes_AssignedToID",
                table: "Routes",
                column: "AssignedToID");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Drivers_AssignedToID",
                table: "Routes",
                column: "AssignedToID",
                principalTable: "Drivers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
