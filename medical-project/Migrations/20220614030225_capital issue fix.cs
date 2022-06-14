using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace medical_project.Migrations
{
    public partial class capitalissuefix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "weight",
                table: "AspNetUsers",
                newName: "Weight");

            migrationBuilder.RenameColumn(
                name: "height",
                table: "AspNetUsers",
                newName: "Height");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "AspNetUsers",
                newName: "weight");

            migrationBuilder.RenameColumn(
                name: "Height",
                table: "AspNetUsers",
                newName: "height");
        }
    }
}
