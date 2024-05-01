using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TexasTaco.Products.Core.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class RemovedLongDescriptionAndAddedRecommendedInProductEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LongDescription",
                table: "Products");

            migrationBuilder.AddColumn<bool>(
                name: "Recommended",
                table: "Products",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Recommended",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "LongDescription",
                table: "Products",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
