using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DeliveryService.Migrations
{
    public partial class TempRouteData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "TempRoutes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DriverID = table.Column<int>(nullable: true),
                    DriversVehicleID = table.Column<int>(nullable: true),
                    ModifiedDeliverByDate = table.Column<DateTime>(nullable: false),
                    RouteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempRoutes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TempRoutes_Drivers_DriverID",
                        column: x => x.DriverID,
                        principalTable: "Drivers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TempRoutes_Vehicles_DriversVehicleID",
                        column: x => x.DriversVehicleID,
                        principalTable: "Vehicles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddColumn<int>(
                name: "AssignedToID",
                table: "Routes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Routes_AssignedToID",
                table: "Routes",
                column: "AssignedToID");

            migrationBuilder.CreateIndex(
                name: "IX_TempRoutes_DriverID",
                table: "TempRoutes",
                column: "DriverID");

            migrationBuilder.CreateIndex(
                name: "IX_TempRoutes_DriversVehicleID",
                table: "TempRoutes",
                column: "DriversVehicleID");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Drivers_AssignedToID",
                table: "Routes",
                column: "AssignedToID",
                principalTable: "Drivers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: "TempRoutes");

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
    }
}
