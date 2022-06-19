using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace medical_project.Migrations
{
    public partial class updatedpharmacyrelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pharmacy_AppUserId",
                table: "Pharmacy");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacy_AppUserId",
                table: "Pharmacy",
                column: "AppUserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pharmacy_AppUserId",
                table: "Pharmacy");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacy_AppUserId",
                table: "Pharmacy",
                column: "AppUserId");
        }
    }
}
