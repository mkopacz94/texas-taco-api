using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TexasTaco.Products.Core.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddedPictureRelationshipToPizeEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prize_Products_ProductId",
                table: "Prize");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Prize",
                table: "Prize");

            migrationBuilder.RenameTable(
                name: "Prize",
                newName: "Prizes");

            migrationBuilder.RenameIndex(
                name: "IX_Prize_ProductId",
                table: "Prizes",
                newName: "IX_Prizes_ProductId");

            migrationBuilder.AddColumn<Guid>(
                name: "PictureId",
                table: "Prizes",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prizes",
                table: "Prizes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Prizes_PictureId",
                table: "Prizes",
                column: "PictureId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Prizes_Pictures_PictureId",
                table: "Prizes",
                column: "PictureId",
                principalTable: "Pictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prizes_Products_ProductId",
                table: "Prizes",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prizes_Pictures_PictureId",
                table: "Prizes");

            migrationBuilder.DropForeignKey(
                name: "FK_Prizes_Products_ProductId",
                table: "Prizes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Prizes",
                table: "Prizes");

            migrationBuilder.DropIndex(
                name: "IX_Prizes_PictureId",
                table: "Prizes");

            migrationBuilder.DropColumn(
                name: "PictureId",
                table: "Prizes");

            migrationBuilder.RenameTable(
                name: "Prizes",
                newName: "Prize");

            migrationBuilder.RenameIndex(
                name: "IX_Prizes_ProductId",
                table: "Prize",
                newName: "IX_Prize_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prize",
                table: "Prize",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Prize_Products_ProductId",
                table: "Prize",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
