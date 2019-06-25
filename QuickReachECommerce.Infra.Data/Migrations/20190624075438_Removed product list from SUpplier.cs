using Microsoft.EntityFrameworkCore.Migrations;

namespace QuickReach.ECommerce.Infra.Data.Migrations
{
    public partial class RemovedproductlistfromSUpplier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Supplier_SupplierID",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_SupplierID",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "SupplierID",
                table: "Product");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SupplierID",
                table: "Product",
                nullable: true);

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
    }
}
