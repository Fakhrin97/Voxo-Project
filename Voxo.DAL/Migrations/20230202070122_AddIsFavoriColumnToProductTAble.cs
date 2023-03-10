using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voxo.DAL.Migrations
{
    public partial class AddIsFavoriColumnToProductTAble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFavori",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFavori",
                table: "Products");
        }
    }
}
