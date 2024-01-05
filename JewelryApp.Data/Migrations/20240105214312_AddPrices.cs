using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelryApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPrices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CoinBahar",
                table: "Price",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CoinGrami",
                table: "Price",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CoinImam",
                table: "Price",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CoinNim",
                table: "Price",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CoinParsian100Sowt",
                table: "Price",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CoinParsian150Sowt",
                table: "Price",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CoinParsian15Gram",
                table: "Price",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CoinParsian1Gram",
                table: "Price",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CoinParsian200Sowt",
                table: "Price",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CoinParsian250Sowt",
                table: "Price",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CoinParsian300Sowt",
                table: "Price",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CoinParsian400Sowt",
                table: "Price",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CoinParsian500Sowt",
                table: "Price",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CoinParsian50Sowt",
                table: "Price",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CoinRob",
                table: "Price",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "UsEuro",
                table: "Price",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoinBahar",
                table: "Price");

            migrationBuilder.DropColumn(
                name: "CoinGrami",
                table: "Price");

            migrationBuilder.DropColumn(
                name: "CoinImam",
                table: "Price");

            migrationBuilder.DropColumn(
                name: "CoinNim",
                table: "Price");

            migrationBuilder.DropColumn(
                name: "CoinParsian100Sowt",
                table: "Price");

            migrationBuilder.DropColumn(
                name: "CoinParsian150Sowt",
                table: "Price");

            migrationBuilder.DropColumn(
                name: "CoinParsian15Gram",
                table: "Price");

            migrationBuilder.DropColumn(
                name: "CoinParsian1Gram",
                table: "Price");

            migrationBuilder.DropColumn(
                name: "CoinParsian200Sowt",
                table: "Price");

            migrationBuilder.DropColumn(
                name: "CoinParsian250Sowt",
                table: "Price");

            migrationBuilder.DropColumn(
                name: "CoinParsian300Sowt",
                table: "Price");

            migrationBuilder.DropColumn(
                name: "CoinParsian400Sowt",
                table: "Price");

            migrationBuilder.DropColumn(
                name: "CoinParsian500Sowt",
                table: "Price");

            migrationBuilder.DropColumn(
                name: "CoinParsian50Sowt",
                table: "Price");

            migrationBuilder.DropColumn(
                name: "CoinRob",
                table: "Price");

            migrationBuilder.DropColumn(
                name: "UsEuro",
                table: "Price");
        }
    }
}
