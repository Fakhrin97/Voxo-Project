using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voxo.DAL.Migrations
{
    public partial class AddPublishedColumnToIEntityTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Published",
                table: "Brands",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Published",
                table: "Brands");
        }
    }
}
