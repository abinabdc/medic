﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace medical_project.Migrations
{
    public partial class asdasdasd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Order");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "OrderProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "OrderProducts");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
