using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelryApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWageType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WageType",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WageType",
                table: "Products");
        }
    }
}
