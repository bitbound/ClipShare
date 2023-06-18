using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClipShare.Server.Migrations;

public partial class NET6 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Keys",
            columns: table => new
            {
                Id = table.Column<string>(type: "TEXT", nullable: false),
                Version = table.Column<int>(type: "INTEGER", nullable: false),
                Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                Use = table.Column<string>(type: "TEXT", nullable: true),
                Algorithm = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                IsX509Certificate = table.Column<bool>(type: "INTEGER", nullable: false),
                DataProtected = table.Column<bool>(type: "INTEGER", nullable: false),
                Data = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Keys", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_PersistedGrants_ConsumedTime",
            table: "PersistedGrants",
            column: "ConsumedTime");

        migrationBuilder.CreateIndex(
            name: "IX_Keys_Use",
            table: "Keys",
            column: "Use");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Keys");

        migrationBuilder.DropIndex(
            name: "IX_PersistedGrants_ConsumedTime",
            table: "PersistedGrants");
    }
}
