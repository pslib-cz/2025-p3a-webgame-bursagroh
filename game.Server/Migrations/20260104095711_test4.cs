using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace game.Server.Migrations
{
    /// <inheritdoc />
    public partial class test4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BlueprintPlayers",
                table: "BlueprintPlayers");

            migrationBuilder.DeleteData(
                table: "BlueprintPlayers",
                keyColumn: "PlayerId",
                keyValue: new Guid("4b1e8a93-7d92-4f7f-80c1-525c345b85e0"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlueprintPlayers",
                table: "BlueprintPlayers",
                columns: new[] { "PlayerId", "BlueprintId" });

            migrationBuilder.CreateIndex(
                name: "IX_BlueprintPlayers_BlueprintId",
                table: "BlueprintPlayers",
                column: "BlueprintId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlueprintPlayers_Blueprints_BlueprintId",
                table: "BlueprintPlayers",
                column: "BlueprintId",
                principalTable: "Blueprints",
                principalColumn: "BlueprintId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlueprintPlayers_Players_PlayerId",
                table: "BlueprintPlayers",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlueprintPlayers_Blueprints_BlueprintId",
                table: "BlueprintPlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_BlueprintPlayers_Players_PlayerId",
                table: "BlueprintPlayers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlueprintPlayers",
                table: "BlueprintPlayers");

            migrationBuilder.DropIndex(
                name: "IX_BlueprintPlayers_BlueprintId",
                table: "BlueprintPlayers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlueprintPlayers",
                table: "BlueprintPlayers",
                column: "PlayerId");

            migrationBuilder.InsertData(
                table: "BlueprintPlayers",
                columns: new[] { "PlayerId", "BlueprintId" },
                values: new object[] { new Guid("4b1e8a93-7d92-4f7f-80c1-525c345b85e0"), 1 });
        }
    }
}
