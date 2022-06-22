using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace medical_project.Migrations
{
    public partial class dbchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "OrderProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "OrderProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "OrderProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "OrderProducts");
        }
    }
}
