using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesApi.Migrations
{
    /// <inheritdoc />
    public partial class FixSalesProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Sales",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Products");
        }
    }
}
