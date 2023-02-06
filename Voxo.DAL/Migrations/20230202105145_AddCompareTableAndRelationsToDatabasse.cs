using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voxo.DAL.Migrations
{
    public partial class AddCompareTableAndRelationsToDatabasse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Compares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Published = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compares", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompareProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Published = table.Column<bool>(type: "bit", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CompareId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompareProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompareProducts_Compares_CompareId",
                        column: x => x.CompareId,
                        principalTable: "Compares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompareProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompareProducts_CompareId",
                table: "CompareProducts",
                column: "CompareId");

            migrationBuilder.CreateIndex(
                name: "IX_CompareProducts_ProductId",
                table: "CompareProducts",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompareProducts");

            migrationBuilder.DropTable(
                name: "Compares");
        }
    }
}
