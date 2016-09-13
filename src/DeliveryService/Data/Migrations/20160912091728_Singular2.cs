using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeliveryService.Data.Migrations
{
    public partial class Singular2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Delivery_Clients_ClientID1",
                table: "Delivery");

            migrationBuilder.DropForeignKey(
                name: "FK_Delivery_Drivers_DriverID",
                table: "Delivery");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryStatus_Drivers_AssignedToId",
                table: "DeliveryStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryStatus_Drivers_PickedUpById",
                table: "DeliveryStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_AspNetUsers_UserId",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Drivers_DriverID",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_AspNetUsers_UserId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Drivers_DriverID",
                table: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teams",
                table: "Teams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Drivers",
                table: "Drivers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clients",
                table: "Clients");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehicle",
                table: "Vehicles",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Team",
                table: "Teams",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Driver",
                table: "Drivers",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Client",
                table: "Clients",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Delivery_Client_ClientID1",
                table: "Delivery",
                column: "ClientID1",
                principalTable: "Clients",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Delivery_Driver_DriverID",
                table: "Delivery",
                column: "DriverID",
                principalTable: "Drivers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryStatus_Driver_AssignedToId",
                table: "DeliveryStatus",
                column: "AssignedToId",
                principalTable: "Drivers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryStatus_Driver_PickedUpById",
                table: "DeliveryStatus",
                column: "PickedUpById",
                principalTable: "Drivers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Driver_AspNetUsers_UserId",
                table: "Drivers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Driver_DriverID",
                table: "Teams",
                column: "DriverID",
                principalTable: "Drivers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_AspNetUsers_UserId",
                table: "Teams",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_Driver_DriverID",
                table: "Vehicles",
                column: "DriverID",
                principalTable: "Drivers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_DriverID",
                table: "Vehicles",
                newName: "IX_Vehicle_DriverID");

            migrationBuilder.RenameIndex(
                name: "IX_Teams_UserId",
                table: "Teams",
                newName: "IX_Team_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Teams_DriverID",
                table: "Teams",
                newName: "IX_Team_DriverID");

            migrationBuilder.RenameIndex(
                name: "IX_Drivers_UserId",
                table: "Drivers",
                newName: "IX_Driver_UserId");

            migrationBuilder.RenameTable(
                name: "Vehicles",
                newName: "Vehicle");

            migrationBuilder.RenameTable(
                name: "Teams",
                newName: "Team");

            migrationBuilder.RenameTable(
                name: "Drivers",
                newName: "Driver");

            migrationBuilder.RenameTable(
                name: "Clients",
                newName: "Client");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Delivery_Client_ClientID1",
                table: "Delivery");

            migrationBuilder.DropForeignKey(
                name: "FK_Delivery_Driver_DriverID",
                table: "Delivery");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryStatus_Driver_AssignedToId",
                table: "DeliveryStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryStatus_Driver_PickedUpById",
                table: "DeliveryStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_Driver_AspNetUsers_UserId",
                table: "Driver");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_Driver_DriverID",
                table: "Team");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_AspNetUsers_UserId",
                table: "Team");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_Driver_DriverID",
                table: "Vehicle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehicle",
                table: "Vehicle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Team",
                table: "Team");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Driver",
                table: "Driver");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Client",
                table: "Client");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicle",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teams",
                table: "Team",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Drivers",
                table: "Driver",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clients",
                table: "Client",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Delivery_Clients_ClientID1",
                table: "Delivery",
                column: "ClientID1",
                principalTable: "Client",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Delivery_Drivers_DriverID",
                table: "Delivery",
                column: "DriverID",
                principalTable: "Driver",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryStatus_Drivers_AssignedToId",
                table: "DeliveryStatus",
                column: "AssignedToId",
                principalTable: "Driver",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryStatus_Drivers_PickedUpById",
                table: "DeliveryStatus",
                column: "PickedUpById",
                principalTable: "Driver",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_AspNetUsers_UserId",
                table: "Driver",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Drivers_DriverID",
                table: "Team",
                column: "DriverID",
                principalTable: "Driver",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_AspNetUsers_UserId",
                table: "Team",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Drivers_DriverID",
                table: "Vehicle",
                column: "DriverID",
                principalTable: "Driver",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.RenameIndex(
                name: "IX_Vehicle_DriverID",
                table: "Vehicle",
                newName: "IX_Vehicles_DriverID");

            migrationBuilder.RenameIndex(
                name: "IX_Team_UserId",
                table: "Team",
                newName: "IX_Teams_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Team_DriverID",
                table: "Team",
                newName: "IX_Teams_DriverID");

            migrationBuilder.RenameIndex(
                name: "IX_Driver_UserId",
                table: "Driver",
                newName: "IX_Drivers_UserId");

            migrationBuilder.RenameTable(
                name: "Vehicle",
                newName: "Vehicles");

            migrationBuilder.RenameTable(
                name: "Team",
                newName: "Teams");

            migrationBuilder.RenameTable(
                name: "Driver",
                newName: "Drivers");

            migrationBuilder.RenameTable(
                name: "Client",
                newName: "Clients");
        }
    }
}
