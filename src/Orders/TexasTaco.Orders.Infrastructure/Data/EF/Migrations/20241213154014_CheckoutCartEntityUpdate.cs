using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TexasTaco.Orders.Infrastructure.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class CheckoutCartEntityUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryAddress");

            migrationBuilder.AddColumn<int>(
                name: "PaymentType",
                table: "CheckoutCarts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PickupLocation",
                table: "CheckoutCarts",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "CheckoutCarts");

            migrationBuilder.DropColumn(
                name: "PickupLocation",
                table: "CheckoutCarts");

            migrationBuilder.CreateTable(
                name: "DeliveryAddress",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CheckoutCartId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    AddressLine = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    City = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PostalCode = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReceiverFullName = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryAddress_CheckoutCarts_CheckoutCartId",
                        column: x => x.CheckoutCartId,
                        principalTable: "CheckoutCarts",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAddress_CheckoutCartId",
                table: "DeliveryAddress",
                column: "CheckoutCartId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAddress_ReceiverFullName",
                table: "DeliveryAddress",
                column: "ReceiverFullName");
        }
    }
}
