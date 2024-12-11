using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TexasTaco.Orders.Infrastructure.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddCheckoutCartEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CheckoutCartId",
                table: "CartProducts",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "CheckoutCart",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CustomerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckoutCart", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DeliveryAddress",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ReceiverFullName = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddressLine = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PostalCode = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    City = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CheckoutCartId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryAddress_CheckoutCart_CheckoutCartId",
                        column: x => x.CheckoutCartId,
                        principalTable: "CheckoutCart",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CartProducts_CheckoutCartId",
                table: "CartProducts",
                column: "CheckoutCartId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckoutCart_CustomerId",
                table: "CheckoutCart",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAddress_CheckoutCartId",
                table: "DeliveryAddress",
                column: "CheckoutCartId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAddress_ReceiverFullName",
                table: "DeliveryAddress",
                column: "ReceiverFullName");

            migrationBuilder.AddForeignKey(
                name: "FK_CartProducts_CheckoutCart_CheckoutCartId",
                table: "CartProducts",
                column: "CheckoutCartId",
                principalTable: "CheckoutCart",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartProducts_CheckoutCart_CheckoutCartId",
                table: "CartProducts");

            migrationBuilder.DropTable(
                name: "DeliveryAddress");

            migrationBuilder.DropTable(
                name: "CheckoutCart");

            migrationBuilder.DropIndex(
                name: "IX_CartProducts_CheckoutCartId",
                table: "CartProducts");

            migrationBuilder.DropColumn(
                name: "CheckoutCartId",
                table: "CartProducts");
        }
    }
}
