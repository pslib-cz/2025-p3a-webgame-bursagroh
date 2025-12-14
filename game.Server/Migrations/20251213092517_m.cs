using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace game.Server.Migrations
{
    /// <inheritdoc />
    public partial class m : Migration
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
                    PlayerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PositionX = table.Column<int>(type: "INTEGER", nullable: false),
                    PositionY = table.Column<int>(type: "INTEGER", nullable: false),
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
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mines", x => x.MineId);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    RecipeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.RecipeId);
                });

            migrationBuilder.CreateTable(
                name: "RecipeTimes",
                columns: table => new
                {
                    RecipeTimeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecipeId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeTimes", x => x.RecipeTimeId);
                });

            migrationBuilder.CreateTable(
                name: "Floors",
                columns: table => new
                {
                    FloorId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BuildingId = table.Column<int>(type: "INTEGER", nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Floors", x => x.FloorId);
                    table.ForeignKey(
                        name: "FK_Floors_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "BuildingId",
                        onDelete: ReferentialAction.Cascade);
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
                name: "Ingrediences",
                columns: table => new
                {
                    IngredienceId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecipeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    IngredienceType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingrediences", x => x.IngredienceId);
                    table.ForeignKey(
                        name: "FK_Ingrediences_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FloorItems",
                columns: table => new
                {
                    FloorItemId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FloorId = table.Column<int>(type: "INTEGER", nullable: false),
                    PositionX = table.Column<int>(type: "INTEGER", nullable: false),
                    PositionY = table.Column<int>(type: "INTEGER", nullable: false),
                    FloorItemType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FloorItems", x => x.FloorItemId);
                    table.ForeignKey(
                        name: "FK_FloorItems_Floors_FloorId",
                        column: x => x.FloorId,
                        principalTable: "Floors",
                        principalColumn: "FloorId",
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
                name: "Players",
                columns: table => new
                {
                    PlayerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Money = table.Column<int>(type: "INTEGER", nullable: false),
                    BankBalance = table.Column<int>(type: "INTEGER", nullable: false),
                    ScreenType = table.Column<int>(type: "INTEGER", nullable: false),
                    BuildingId = table.Column<int>(type: "INTEGER", nullable: true),
                    FloorItemId = table.Column<int>(type: "INTEGER", nullable: true),
                    Capacity = table.Column<int>(type: "INTEGER", nullable: false),
                    Seed = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_Players_FloorItems_FloorItemId",
                        column: x => x.FloorItemId,
                        principalTable: "FloorItems",
                        principalColumn: "FloorItemId");
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
                table: "Buildings",
                columns: new[] { "BuildingId", "BuildingType", "Height", "IsBossDefeated", "PlayerId", "PositionX", "PositionY", "ReachedHeight" },
                values: new object[] { 69, 0, null, false, new Guid("4b1e8a93-7d92-4f7f-80c1-525c345b85e0"), 0, 0, null });

            migrationBuilder.InsertData(
                table: "InventoryItems",
                columns: new[] { "InventoryItemId", "IsInBank", "ItemInstanceId", "PlayerId" },
                values: new object[] { 52, true, 85, new Guid("4b1e8a93-7d92-4f7f-80c1-525c345b85e0") });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemId", "Damage", "Description", "ItemInstanceId", "ItemType", "MaxDurability", "Name", "Weight" },
                values: new object[,]
                {
                    { 1, 0, null, null, 0, 0, "Rock", 0 },
                    { 2, 0, null, null, 0, 0, "Iron Ore", 0 },
                    { 3, 0, null, null, 0, 0, "Copper Ore", 0 },
                    { 4, 0, null, null, 0, 0, "Silver Ore", 0 },
                    { 5, 0, null, null, 0, 0, "Gold Ore", 0 },
                    { 6, 0, null, null, 0, 0, "Unobtainium Ore", 0 }
                });

            migrationBuilder.InsertData(
                table: "Mines",
                column: "MineId",
                value: 1);

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "PlayerId", "BankBalance", "BuildingId", "Capacity", "FloorItemId", "Money", "Name", "ScreenType", "Seed" },
                values: new object[] { new Guid("4b1e8a93-7d92-4f7f-80c1-525c345b85e0"), 0, null, 10, null, 100, "Seeded Player", 0, 252 });

            migrationBuilder.InsertData(
                table: "RecipeTimes",
                columns: new[] { "RecipeTimeId", "EndTime", "PlayerId", "RecipeId", "StartTime" },
                values: new object[] { 1, new DateTime(2025, 12, 13, 11, 0, 0, 0, DateTimeKind.Utc), new Guid("4b1e8a93-7d92-4f7f-80c1-525c345b85e0"), 1, new DateTime(2025, 12, 13, 10, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "RecipeId", "Name" },
                values: new object[] { 1, "Hamburger" });

            migrationBuilder.InsertData(
                table: "Blocks",
                columns: new[] { "BlockId", "BlockType", "ItemId", "MaxAmount", "MinAmount" },
                values: new object[,]
                {
                    { 1, 1, 1, 3, 1 },
                    { 2, 3, 2, 1, 1 },
                    { 3, 4, 3, 1, 1 },
                    { 4, 5, 4, 1, 1 },
                    { 5, 6, 5, 1, 1 },
                    { 6, 7, 6, 1, 1 }
                });

            migrationBuilder.InsertData(
                table: "Floors",
                columns: new[] { "FloorId", "BuildingId", "Level" },
                values: new object[] { 6, 69, 0 });

            migrationBuilder.InsertData(
                table: "Ingrediences",
                columns: new[] { "IngredienceId", "IngredienceType", "Order", "RecipeId" },
                values: new object[,]
                {
                    { 1, 2, 1, 1 },
                    { 2, 0, 2, 1 },
                    { 3, 3, 3, 1 }
                });

            migrationBuilder.InsertData(
                table: "MineLayers",
                columns: new[] { "MineLayerID", "Depth", "MineId" },
                values: new object[] { 1, 0, 1 });

            migrationBuilder.InsertData(
                table: "FloorItems",
                columns: new[] { "FloorItemId", "FloorId", "FloorItemType", "PositionX", "PositionY" },
                values: new object[] { 85, 6, 0, 0, 0 });

            migrationBuilder.CreateIndex(
                name: "IX_Blocks_ItemId",
                table: "Blocks",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FloorItems_FloorId",
                table: "FloorItems",
                column: "FloorId");

            migrationBuilder.CreateIndex(
                name: "IX_Floors_BuildingId",
                table: "Floors",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingrediences_RecipeId",
                table: "Ingrediences",
                column: "RecipeId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Players_FloorItemId",
                table: "Players",
                column: "FloorItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ingrediences");

            migrationBuilder.DropTable(
                name: "InventoryItems");

            migrationBuilder.DropTable(
                name: "MineBlocks");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "RecipeTimes");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "Blocks");

            migrationBuilder.DropTable(
                name: "MineLayers");

            migrationBuilder.DropTable(
                name: "FloorItems");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Mines");

            migrationBuilder.DropTable(
                name: "Floors");

            migrationBuilder.DropTable(
                name: "ItemInstances");

            migrationBuilder.DropTable(
                name: "Buildings");
        }
    }
}
