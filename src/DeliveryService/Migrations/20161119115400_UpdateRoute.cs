using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DeliveryService.Migrations
{
    public partial class UpdateRoute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TempRoutes");

            migrationBuilder.AddColumn<int>(
                name: "VehicleID",
                table: "Routes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehicleID",
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

            migrationBuilder.CreateIndex(
                name: "IX_TempRoutes_DriverID",
                table: "TempRoutes",
                column: "DriverID");

            migrationBuilder.CreateIndex(
                name: "IX_TempRoutes_DriversVehicleID",
                table: "TempRoutes",
                column: "DriversVehicleID");
        }
    }
}
