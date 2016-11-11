using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeliveryService.Migrations
{
    public partial class UpdateDelivery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Routes_RouteID",
                table: "Deliveries");

            migrationBuilder.AlterColumn<int>(
                name: "RouteID",
                table: "Deliveries",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Routes_RouteID",
                table: "Deliveries",
                column: "RouteID",
                principalTable: "Routes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Routes_RouteID",
                table: "Deliveries");

            migrationBuilder.AlterColumn<int>(
                name: "RouteID",
                table: "Deliveries",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Routes_RouteID",
                table: "Deliveries",
                column: "RouteID",
                principalTable: "Routes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
