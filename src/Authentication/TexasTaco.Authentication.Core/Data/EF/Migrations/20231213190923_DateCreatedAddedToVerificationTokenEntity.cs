using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TexasTaco.Authentication.Core.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class DateCreatedAddedToVerificationTokenEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "VerificationTokens",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "VerificationTokens");
        }
    }
}
