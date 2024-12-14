﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TexasTaco.Orders.Infrastructure.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddPointsCollectedOutboxMessageEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PointsCollectedOutboxMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MessageBody = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Published = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    MessageStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointsCollectedOutboxMessages", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PointsCollectedOutboxMessages");
        }
    }
}
