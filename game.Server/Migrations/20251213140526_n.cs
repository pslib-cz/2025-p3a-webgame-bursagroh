using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace game.Server.Migrations
{
    /// <inheritdoc />
    public partial class n : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "InventoryItemId",
                keyValue: 52);

            migrationBuilder.CreateTable(
                name: "Blueprints",
                columns: table => new
                {
                    BlueprintId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blueprints", x => x.BlueprintId);
                });

            migrationBuilder.CreateTable(
                name: "Craftings",
                columns: table => new
                {
                    CraftingId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BlueprintId = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Craftings", x => x.CraftingId);
                    table.ForeignKey(
                        name: "FK_Craftings_Blueprints_BlueprintId",
                        column: x => x.BlueprintId,
                        principalTable: "Blueprints",
                        principalColumn: "BlueprintId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Craftings_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Blueprints",
                columns: new[] { "BlueprintId", "ItemId", "Price" },
                values: new object[] { 1, 10, 5 });

            migrationBuilder.InsertData(
                table: "Craftings",
                columns: new[] { "CraftingId", "Amount", "BlueprintId", "ItemId" },
                values: new object[] { 1, 3, 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Craftings_BlueprintId",
                table: "Craftings",
                column: "BlueprintId");

            migrationBuilder.CreateIndex(
                name: "IX_Craftings_ItemId",
                table: "Craftings",
                column: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Craftings");

            migrationBuilder.DropTable(
                name: "Blueprints");

            migrationBuilder.InsertData(
                table: "InventoryItems",
                columns: new[] { "InventoryItemId", "IsInBank", "ItemInstanceId", "PlayerId" },
                values: new object[] { 52, true, 85, new Guid("4b1e8a93-7d92-4f7f-80c1-525c345b85e0") });
        }
    }
}
