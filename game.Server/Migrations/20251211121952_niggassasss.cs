using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace game.Server.Migrations
{
    /// <inheritdoc />
    public partial class niggassasss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    BuildingId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PositionX = table.Column<int>(type: "INTEGER", nullable: false),
                    PositionY = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    BuildingType = table.Column<int>(type: "INTEGER", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: true),
                    ReachedHeight = table.Column<int>(type: "INTEGER", nullable: true),
                    IsBossDefeated = table.Column<bool>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.BuildingId);
                });

            migrationBuilder.CreateTable(
                name: "ItemInstances",
                columns: table => new
                {
                    ItemInstanceId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    Durability = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemInstances", x => x.ItemInstanceId);
                });

            migrationBuilder.CreateTable(
                name: "Mines",
                columns: table => new
                {
                    MineId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mines", x => x.MineId);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    PlayerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PositionX = table.Column<int>(type: "INTEGER", nullable: false),
                    PositionY = table.Column<int>(type: "INTEGER", nullable: false),
                    Money = table.Column<int>(type: "INTEGER", nullable: false),
                    BankBalance = table.Column<int>(type: "INTEGER", nullable: false),
                    ScreenType = table.Column<int>(type: "INTEGER", nullable: false),
                    BuildingID = table.Column<int>(type: "INTEGER", nullable: true),
                    FloorItemID = table.Column<int>(type: "INTEGER", nullable: true),
                    Capacity = table.Column<int>(type: "INTEGER", nullable: false),
                    Seed = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.PlayerId);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ItemType = table.Column<int>(type: "INTEGER", nullable: false),
                    Weight = table.Column<int>(type: "INTEGER", nullable: false),
                    Damage = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxDurability = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemId1 = table.Column<int>(type: "INTEGER", nullable: true),
                    ItemInstanceId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_Items_ItemInstances_ItemInstanceId",
                        column: x => x.ItemInstanceId,
                        principalTable: "ItemInstances",
                        principalColumn: "ItemInstanceId");
                    table.ForeignKey(
                        name: "FK_Items_Items_ItemId1",
                        column: x => x.ItemId1,
                        principalTable: "Items",
                        principalColumn: "ItemId");
                });

            migrationBuilder.CreateTable(
                name: "MineLayers",
                columns: table => new
                {
                    MineLayerID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MineId = table.Column<int>(type: "INTEGER", nullable: false),
                    Depth = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MineLayers", x => x.MineLayerID);
                    table.ForeignKey(
                        name: "FK_MineLayers_Mines_MineId",
                        column: x => x.MineId,
                        principalTable: "Mines",
                        principalColumn: "MineId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItems",
                columns: table => new
                {
                    InventoryItemId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ItemInstanceId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsInBank = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItems", x => x.InventoryItemId);
                    table.ForeignKey(
                        name: "FK_InventoryItems_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Blocks",
                columns: table => new
                {
                    BlockId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BlockType = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    MinAmount = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxAmount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blocks", x => x.BlockId);
                    table.ForeignKey(
                        name: "FK_Blocks_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MineBlocks",
                columns: table => new
                {
                    MineBlockId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MineLayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Index = table.Column<int>(type: "INTEGER", nullable: false),
                    BlockId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MineBlocks", x => x.MineBlockId);
                    table.ForeignKey(
                        name: "FK_MineBlocks_Blocks_BlockId",
                        column: x => x.BlockId,
                        principalTable: "Blocks",
                        principalColumn: "BlockId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MineBlocks_MineLayers_MineLayerId",
                        column: x => x.MineLayerId,
                        principalTable: "MineLayers",
                        principalColumn: "MineLayerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemId", "Damage", "Description", "ItemId1", "ItemInstanceId", "ItemType", "MaxDurability", "Name", "Weight" },
                values: new object[,]
                {
                    { 101, 0, null, null, null, 0, 0, "Stone Resource", 0 },
                    { 202, 0, null, null, null, 0, 0, "Ore Resource", 0 }
                });

            migrationBuilder.InsertData(
                table: "Mines",
                columns: new[] { "MineId", "PlayerId" },
                values: new object[] { 1, new Guid("4b1e8a93-7d92-4f7f-80c1-525c345b85e0") });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "PlayerId", "BankBalance", "BuildingID", "Capacity", "FloorItemID", "Money", "Name", "PositionX", "PositionY", "ScreenType", "Seed" },
                values: new object[] { new Guid("4b1e8a93-7d92-4f7f-80c1-525c345b85e0"), 0, null, 10, null, 100, "Seeded Player", 0, 0, 0, 252 });

            migrationBuilder.InsertData(
                table: "Blocks",
                columns: new[] { "BlockId", "BlockType", "ItemId", "MaxAmount", "MinAmount" },
                values: new object[,]
                {
                    { 1, 1, 101, 3, 1 },
                    { 2, 0, 202, 1, 1 }
                });

            migrationBuilder.InsertData(
                table: "InventoryItems",
                columns: new[] { "InventoryItemId", "IsInBank", "ItemInstanceId", "PlayerId" },
                values: new object[] { 52, true, 85, new Guid("4b1e8a93-7d92-4f7f-80c1-525c345b85e0") });

            migrationBuilder.InsertData(
                table: "MineLayers",
                columns: new[] { "MineLayerID", "Depth", "MineId" },
                values: new object[] { 1, 0, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Blocks_ItemId",
                table: "Blocks",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_PlayerId",
                table: "InventoryItems",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemId1",
                table: "Items",
                column: "ItemId1");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemInstanceId",
                table: "Items",
                column: "ItemInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_MineBlocks_BlockId",
                table: "MineBlocks",
                column: "BlockId");

            migrationBuilder.CreateIndex(
                name: "IX_MineBlocks_MineLayerId",
                table: "MineBlocks",
                column: "MineLayerId");

            migrationBuilder.CreateIndex(
                name: "IX_MineLayers_MineId",
                table: "MineLayers",
                column: "MineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropTable(
                name: "InventoryItems");

            migrationBuilder.DropTable(
                name: "MineBlocks");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Blocks");

            migrationBuilder.DropTable(
                name: "MineLayers");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Mines");

            migrationBuilder.DropTable(
                name: "ItemInstances");
        }
    }
}
