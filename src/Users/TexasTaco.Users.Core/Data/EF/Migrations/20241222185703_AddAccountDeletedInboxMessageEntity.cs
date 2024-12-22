using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TexasTaco.Users.Core.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountDeletedInboxMessageEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MessageBody",
                table: "UserUpdatedOutboxMessages",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "MessageBody",
                table: "PointsCollectedInboxMessages",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "MessageBody",
                table: "AccountCreatedInboxMessages",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AccountDeletedInboxMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Received = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Processed = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    MessageId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MessageBody = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MessageStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountDeletedInboxMessages", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountDeletedInboxMessages");

            migrationBuilder.UpdateData(
                table: "UserUpdatedOutboxMessages",
                keyColumn: "MessageBody",
                keyValue: null,
                column: "MessageBody",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "MessageBody",
                table: "UserUpdatedOutboxMessages",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "PointsCollectedInboxMessages",
                keyColumn: "MessageBody",
                keyValue: null,
                column: "MessageBody",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "MessageBody",
                table: "PointsCollectedInboxMessages",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AccountCreatedInboxMessages",
                keyColumn: "MessageBody",
                keyValue: null,
                column: "MessageBody",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "MessageBody",
                table: "AccountCreatedInboxMessages",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
