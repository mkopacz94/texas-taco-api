using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TexasTaco.Authentication.Core.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedAccountCreatedOutboxWithDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "AccountsCreatedOutbox",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Published",
                table: "AccountsCreatedOutbox",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "AccountsCreatedOutbox");

            migrationBuilder.DropColumn(
                name: "Published",
                table: "AccountsCreatedOutbox");
        }
    }
}
