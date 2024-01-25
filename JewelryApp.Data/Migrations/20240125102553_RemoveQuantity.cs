using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelryApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "InvoiceItem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "InvoiceItem",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
