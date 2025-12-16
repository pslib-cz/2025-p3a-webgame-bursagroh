using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace game.Server.Migrations
{
    /// <inheritdoc />
    public partial class tesik : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_FloorItems_FloorItemId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_FloorItemId",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "FloorItemId",
                table: "Players",
                newName: "SubPositionY");

            migrationBuilder.RenameColumn(
                name: "BuildingId",
                table: "Players",
                newName: "SubPositionX");

            migrationBuilder.AddColumn<int>(
                name: "PositionX",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PositionY",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "PlayerId",
                keyValue: new Guid("4b1e8a93-7d92-4f7f-80c1-525c345b85e0"),
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 0, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PositionX",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "PositionY",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "SubPositionY",
                table: "Players",
                newName: "FloorItemId");

            migrationBuilder.RenameColumn(
                name: "SubPositionX",
                table: "Players",
                newName: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_FloorItemId",
                table: "Players",
                column: "FloorItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_FloorItems_FloorItemId",
                table: "Players",
                column: "FloorItemId",
                principalTable: "FloorItems",
                principalColumn: "FloorItemId");
        }
    }
}
