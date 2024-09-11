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
            migrationBuilder.AddColumn<Guid>(
                name: "PictureId",
                table: "Prizes",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prizes_Pictures_PictureId",
                table: "Prizes");

            migrationBuilder.DropIndex(
                name: "IX_Prizes_PictureId",
                table: "Prizes");

            migrationBuilder.DropColumn(
                name: "PictureId",
                table: "Prizes");
        }
    }
}
