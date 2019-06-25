using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QuickReach.ECommerce.Infra.Data.Migrations
{
    public partial class EditedSupplieragain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SupplierID",
                table: "Product",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Supplier",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 40, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplier", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_SupplierID",
                table: "Product",
                column: "SupplierID");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Supplier_SupplierID",
                table: "Product",
                column: "SupplierID",
                principalTable: "Supplier",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Supplier_SupplierID",
                table: "Product");

            migrationBuilder.DropTable(
                name: "Supplier");

            migrationBuilder.DropIndex(
                name: "IX_Product_SupplierID",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "SupplierID",
                table: "Product");
        }
    }
}
