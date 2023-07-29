using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelryApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveForApiKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ApiKeys",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ApiKeys");
        }
    }
}
