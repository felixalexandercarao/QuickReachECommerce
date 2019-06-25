using Microsoft.EntityFrameworkCore.Migrations;

namespace QuickReach.ECommerce.Infra.Data.Migrations
{
    public partial class AddIsActivetoCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Category",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Category");
        }
    }
}
