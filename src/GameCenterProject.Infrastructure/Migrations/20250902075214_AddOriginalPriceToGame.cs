using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameCenterProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOriginalPriceToGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "OriginalPriceAmount",
                table: "Games",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginalPriceCurrency",
                table: "Games",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalPriceAmount",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "OriginalPriceCurrency",
                table: "Games");
        }
    }
}
