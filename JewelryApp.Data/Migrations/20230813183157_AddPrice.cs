using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelryApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "BuyDateTime",
                table: "Invoices",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Gold18K = table.Column<double>(type: "float", nullable: false),
                    Gold24K = table.Column<double>(type: "float", nullable: false),
                    GoldOunce = table.Column<double>(type: "float", nullable: false),
                    OldCoin = table.Column<double>(type: "float", nullable: false),
                    NewCoin = table.Column<double>(type: "float", nullable: false),
                    HalfCoin = table.Column<double>(type: "float", nullable: false),
                    QuarterCoin = table.Column<double>(type: "float", nullable: false),
                    GramCoin = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BuyDateTime",
                table: "Invoices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
