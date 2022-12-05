using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contabilizacao.Data.Migrations
{
    public partial class weight : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "Products",
                newName: "WeightOrVolume");

            migrationBuilder.AddColumn<bool>(
                name: "HasVolume",
                table: "Products",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasVolume",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "WeightOrVolume",
                table: "Products",
                newName: "Weight");
        }
    }
}
