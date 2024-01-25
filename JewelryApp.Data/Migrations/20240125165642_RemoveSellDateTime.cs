using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelryApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSellDateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellDateTime",
                table: "Product");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SellDateTime",
                table: "Product",
                type: "datetime2",
                nullable: true);
        }
    }
}
