using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TexasTaco.Authentication.Core.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class ChangedDateCreatedToExpirationDateInVerificationToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "VerificationTokens",
                newName: "ExpirationDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpirationDate",
                table: "VerificationTokens",
                newName: "DateCreated");
        }
    }
}
