using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using DeliveryService.Models.Entities;

namespace DeliveryService.Migrations
{
    public partial class Delivery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemSize",
                table: "Deliveries",
                nullable: false,
                defaultValue: ItemSize.Small);

            migrationBuilder.AddColumn<double>(
                name: "ItemWeight",
                table: "Deliveries",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemSize",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "ItemWeight",
                table: "Deliveries");
        }
    }
}
