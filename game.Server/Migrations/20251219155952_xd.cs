using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace game.Server.Migrations
{
    /// <inheritdoc />
    public partial class xd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SubPositionY",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SubPositionX",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Enemies",
                columns: table => new
                {
                    EnemyId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Health = table.Column<int>(type: "INTEGER", nullable: false),
                    FloorItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemInstanceId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enemies", x => x.EnemyId);
                    table.ForeignKey(
                        name: "FK_Enemies_FloorItems_FloorItemId",
                        column: x => x.FloorItemId,
                        principalTable: "FloorItems",
                        principalColumn: "FloorItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chests",
                columns: table => new
                {
                    ChestId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FloorItemId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chests", x => x.ChestId);
                    table.ForeignKey(
                        name: "FK_Chests_FloorItems_FloorItemId",
                        column: x => x.FloorItemId,
                        principalTable: "FloorItems",
                        principalColumn: "FloorItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "PlayerId",
                keyValue: new Guid("4b1e8a93-7d92-4f7f-80c1-525c345b85e0"),
                columns: new[] { "SubPositionX", "SubPositionY" },
                values: new object[] { 0, 0 });

            migrationBuilder.CreateIndex(
                name: "IX_Enemies_FloorItemId",
                table: "Enemies",
                column: "FloorItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chests_FloorItemId",
                table: "Chests",
                column: "FloorItemId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enemies");

            migrationBuilder.DropTable(
                name: "Chests");

            migrationBuilder.AlterColumn<int>(
                name: "SubPositionY",
                table: "Players",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "SubPositionX",
                table: "Players",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "PlayerId",
                keyValue: new Guid("4b1e8a93-7d92-4f7f-80c1-525c345b85e0"),
                columns: new[] { "SubPositionX", "SubPositionY" },
                values: new object[] { null, null });
        }
    }
}
