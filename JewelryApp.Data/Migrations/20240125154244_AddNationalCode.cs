using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelryApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNationalCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NationalCode",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NationalCode",
                table: "Customer");
        }
    }
}
