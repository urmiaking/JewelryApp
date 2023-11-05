using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelryApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUsDollar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "UsDollar",
                table: "Prices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsDollar",
                table: "Prices");
        }
    }
}
