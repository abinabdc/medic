using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace medical_project.Migrations
{
    public partial class changesinname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Order",
                newName: "PayType");

            migrationBuilder.RenameColumn(
                name: "PaymentType",
                table: "Order",
                newName: "PayStatus");

            migrationBuilder.RenameColumn(
                name: "PaymentStatus",
                table: "Order",
                newName: "OrderStatus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PayType",
                table: "Order",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "PayStatus",
                table: "Order",
                newName: "PaymentType");

            migrationBuilder.RenameColumn(
                name: "OrderStatus",
                table: "Order",
                newName: "PaymentStatus");
        }
    }
}
