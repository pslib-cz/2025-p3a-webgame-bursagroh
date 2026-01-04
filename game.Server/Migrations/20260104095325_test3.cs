using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace game.Server.Migrations
{
    /// <inheritdoc />
    public partial class test3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlueprintPlayers",
                columns: table => new
                {
                    PlayerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    BlueprintId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlueprintPlayers", x => x.PlayerId);
                });

            migrationBuilder.InsertData(
                table: "BlueprintPlayers",
                columns: new[] { "PlayerId", "BlueprintId" },
                values: new object[] { new Guid("4b1e8a93-7d92-4f7f-80c1-525c345b85e0"), 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlueprintPlayers");
        }
    }
}
