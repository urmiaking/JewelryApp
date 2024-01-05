using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelryApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPrices2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CoinParsian2Gram",
                table: "Price",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoinParsian2Gram",
                table: "Price");
        }
    }
}
