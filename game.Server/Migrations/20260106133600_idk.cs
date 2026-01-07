using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace game.Server.Migrations
{
    /// <inheritdoc />
    public partial class idk : Migration
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
                    ChangeOfGenerating = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemId);
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
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_Blueprints_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
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
                    table.ForeignKey(
                        name: "FK_ItemInstances_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
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
                name: "Players",
                columns: table => new
                {
                    PlayerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Money = table.Column<int>(type: "INTEGER", nullable: false),
                    BankBalance = table.Column<int>(type: "INTEGER", nullable: false),
                    ScreenType = table.Column<int>(type: "INTEGER", nullable: false),
                    PositionX = table.Column<int>(type: "INTEGER", nullable: false),
                    PositionY = table.Column<int>(type: "INTEGER", nullable: false),
                    SubPositionX = table.Column<int>(type: "INTEGER", nullable: false),
                    SubPositionY = table.Column<int>(type: "INTEGER", nullable: false),
                    FloorId = table.Column<int>(type: "INTEGER", nullable: true),
                    Capacity = table.Column<int>(type: "INTEGER", nullable: false),
                    Seed = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_Players_Floors_FloorId",
                        column: x => x.FloorId,
                        principalTable: "Floors",
                        principalColumn: "FloorId");
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
                    ItemInstanceId = table.Column<int>(type: "INTEGER", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_FloorItems_ItemInstances_ItemInstanceId",
                        column: x => x.ItemInstanceId,
                        principalTable: "ItemInstances",
                        principalColumn: "ItemInstanceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MineBlocks",
                columns: table => new
                {
                    MineBlockId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MineLayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Index = table.Column<int>(type: "INTEGER", nullable: false),
                    BlockId = table.Column<int>(type: "INTEGER", nullable: false),
                    Health = table.Column<int>(type: "INTEGER", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "BlueprintPlayers",
                columns: table => new
                {
                    PlayerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    BlueprintId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlueprintPlayers", x => new { x.PlayerId, x.BlueprintId });
                    table.ForeignKey(
                        name: "FK_BlueprintPlayers_Blueprints_BlueprintId",
                        column: x => x.BlueprintId,
                        principalTable: "Blueprints",
                        principalColumn: "BlueprintId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlueprintPlayers_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
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
                        name: "FK_InventoryItems_ItemInstances_ItemInstanceId",
                        column: x => x.ItemInstanceId,
                        principalTable: "ItemInstances",
                        principalColumn: "ItemInstanceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryItems_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enemies",
                columns: table => new
                {
                    EnemyId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Health = table.Column<int>(type: "INTEGER", nullable: false),
                    EnemyType = table.Column<int>(type: "INTEGER", nullable: false),
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

            migrationBuilder.InsertData(
                table: "Buildings",
                columns: new[] { "BuildingId", "BuildingType", "Height", "IsBossDefeated", "PlayerId", "PositionX", "PositionY", "ReachedHeight" },
                values: new object[] { 69, 0, null, false, new Guid("4b1e8a93-7d92-4f7f-80c1-525c345b85e0"), 0, 0, null });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemId", "ChangeOfGenerating", "Damage", "Description", "ItemType", "MaxDurability", "Name", "Weight" },
                values: new object[,]
                {
                    { 1, 20, 0, "Wooden Frame", 4, 0, "Wooden Frame", 0 },
                    { 2, 80, 0, "Rock", 4, 0, "Rock", 0 },
                    { 3, 15, 0, "Copper Ore", 4, 0, "Copper Ore", 0 },
                    { 4, 15, 0, "Iron Ore", 4, 0, "Iron Ore", 0 },
                    { 5, 10, 0, "Silver Ore", 4, 0, "Silver Ore", 0 },
                    { 6, 10, 0, "Gold Ore", 4, 0, "Gold Ore", 0 },
                    { 7, 2, 0, "Unobtainium Ore", 4, 0, "Unobtainium Ore", 0 },
                    { 10, 0, 10, "Wooden Sword", 0, 20, "Wooden Sword", 1 },
                    { 11, 0, 20, "Rock Sword", 0, 40, "Rock Sword", 1 },
                    { 12, 0, 30, "Copper Sword", 0, 60, "Copper Sword", 1 },
                    { 13, 0, 40, "Iron Sword", 0, 80, "Iron Sword", 1 },
                    { 14, 0, 50, "Silver Sword", 0, 100, "Silver Sword", 1 },
                    { 15, 0, 60, "Gold Sword", 0, 120, "Gold Sword", 1 },
                    { 16, 0, 70, "Unobtainium Sword", 0, 240, "Unobtainium Sword", 1 },
                    { 20, 0, 1, "Wooden Axe", 1, 20, "Wooden Axe", 1 },
                    { 21, 0, 2, "Rock Axe", 1, 40, "Rock Axe", 1 },
                    { 22, 0, 3, "Copper Axe", 1, 60, "Copper Axe", 1 },
                    { 23, 0, 4, "Iron Axe", 1, 80, "Iron Axe", 1 },
                    { 24, 0, 5, "Silver Axe", 1, 100, "Silver Axe", 1 },
                    { 25, 0, 6, "Gold Axe", 1, 120, "Gold Axe", 1 },
                    { 26, 0, 7, "Unobtainium Axe", 1, 240, "Unobtainium Axe", 1 },
                    { 30, 0, 1, "Wooden Pickaxe", 2, 20, "Wooden Pickaxe", 1 },
                    { 31, 0, 2, "Rock Pickaxe", 2, 40, "Rock Pickaxe", 1 },
                    { 32, 0, 3, "Copper Pickaxe", 2, 60, "Copper Pickaxe", 1 },
                    { 33, 0, 4, "Iron Pickaxe", 2, 80, "Iron Pickaxe", 1 },
                    { 34, 0, 5, "Silver Pickaxe", 2, 100, "Silver Pickaxe", 1 },
                    { 35, 0, 6, "Gold Pickaxe", 2, 120, "Gold Pickaxe", 1 },
                    { 36, 0, 7, "Unobtainium Pickaxe", 2, 240, "Unobtainium Pickaxe", 1 },
                    { 39, 0, 1, "Rented Pickaxe", 2, 5, "Rented Pickaxe", 1 }
                });

            migrationBuilder.InsertData(
                table: "Mines",
                columns: new[] { "MineId", "PlayerId" },
                values: new object[] { 1, new Guid("00000000-0000-0000-0000-000000000000") });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "PlayerId", "BankBalance", "Capacity", "FloorId", "Money", "Name", "PositionX", "PositionY", "ScreenType", "Seed", "SubPositionX", "SubPositionY" },
                values: new object[] { new Guid("4b1e8a93-7d92-4f7f-80c1-525c345b85e0"), 0, 10, null, 100, "Seeded Player", 0, 0, 0, 252, 0, 0 });

            migrationBuilder.InsertData(
                table: "RecipeTimes",
                columns: new[] { "RecipeTimeId", "EndTime", "PlayerId", "RecipeId", "StartTime" },
                values: new object[] { 1, new DateTime(2025, 12, 13, 11, 0, 0, 0, DateTimeKind.Utc), new Guid("4b1e8a93-7d92-4f7f-80c1-525c345b85e0"), 1, new DateTime(2025, 12, 13, 10, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "RecipeId", "Name" },
                values: new object[,]
                {
                    { 1, "Classic Hamburger" },
                    { 2, "Cheeseburger" },
                    { 3, "Bacon Burger" },
                    { 4, "Veggie Deluxe" },
                    { 5, "BLT Sandwich" },
                    { 6, "Chicken Sandwich" },
                    { 7, "The Works Burger" },
                    { 8, "Simple Salad Burger" },
                    { 9, "Triple Meat Stack" },
                    { 10, "Saucy Bacon Tomato Burger" },
                    { 11, "Bacon Cheeseburger Deluxe" }
                });

            migrationBuilder.InsertData(
                table: "Blocks",
                columns: new[] { "BlockId", "BlockType", "ItemId", "MaxAmount", "MinAmount" },
                values: new object[,]
                {
                    { 1, 2, 1, 1, 1 },
                    { 2, 1, 2, 1, 1 },
                    { 3, 4, 3, 1, 1 },
                    { 4, 3, 4, 1, 1 },
                    { 5, 5, 5, 1, 1 },
                    { 6, 6, 6, 1, 1 },
                    { 7, 7, 7, 1, 1 }
                });

            migrationBuilder.InsertData(
                table: "Blueprints",
                columns: new[] { "BlueprintId", "ItemId", "Price" },
                values: new object[] { 1, 10, 5 });

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
                    { 3, 3, 3, 1 },
                    { 4, 2, 1, 2 },
                    { 5, 0, 2, 2 },
                    { 6, 7, 3, 2 },
                    { 7, 3, 4, 2 },
                    { 8, 2, 1, 3 },
                    { 9, 0, 2, 3 },
                    { 10, 6, 3, 3 },
                    { 11, 3, 4, 3 },
                    { 12, 2, 1, 4 },
                    { 13, 1, 2, 4 },
                    { 14, 4, 3, 4 },
                    { 15, 5, 4, 4 },
                    { 16, 1, 5, 4 },
                    { 17, 3, 6, 4 },
                    { 18, 2, 1, 5 },
                    { 19, 6, 2, 5 },
                    { 20, 1, 3, 5 },
                    { 21, 4, 4, 5 },
                    { 22, 3, 5, 5 },
                    { 23, 2, 1, 6 },
                    { 24, 5, 2, 6 },
                    { 25, 0, 3, 6 },
                    { 26, 1, 4, 6 },
                    { 27, 3, 5, 6 },
                    { 28, 2, 1, 7 },
                    { 29, 5, 2, 7 },
                    { 30, 4, 3, 7 },
                    { 31, 0, 4, 7 },
                    { 32, 6, 5, 7 },
                    { 33, 1, 6, 7 },
                    { 34, 3, 7, 7 },
                    { 35, 2, 1, 8 },
                    { 36, 1, 2, 8 },
                    { 37, 0, 3, 8 },
                    { 38, 3, 4, 8 },
                    { 39, 2, 1, 9 },
                    { 40, 0, 2, 9 },
                    { 41, 0, 3, 9 },
                    { 42, 0, 4, 9 },
                    { 43, 3, 5, 9 },
                    { 44, 2, 1, 10 },
                    { 45, 5, 2, 10 },
                    { 46, 0, 3, 10 },
                    { 47, 6, 4, 10 },
                    { 48, 4, 5, 10 },
                    { 49, 3, 6, 10 },
                    { 50, 2, 1, 11 },
                    { 51, 1, 2, 11 },
                    { 52, 4, 3, 11 },
                    { 53, 7, 4, 11 },
                    { 54, 0, 5, 11 },
                    { 55, 6, 6, 11 },
                    { 56, 3, 7, 11 }
                });

            migrationBuilder.InsertData(
                table: "MineLayers",
                columns: new[] { "MineLayerID", "Depth", "MineId" },
                values: new object[] { 1, 0, 1 });

            migrationBuilder.InsertData(
                table: "Craftings",
                columns: new[] { "CraftingId", "Amount", "BlueprintId", "ItemId" },
                values: new object[] { 1, 3, 1, 1 });

            migrationBuilder.InsertData(
                table: "FloorItems",
                columns: new[] { "FloorItemId", "FloorId", "FloorItemType", "ItemInstanceId", "PositionX", "PositionY" },
                values: new object[] { 85, 6, 0, null, 0, 0 });

            migrationBuilder.CreateIndex(
                name: "IX_Blocks_ItemId",
                table: "Blocks",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BlueprintPlayers_BlueprintId",
                table: "BlueprintPlayers",
                column: "BlueprintId");

            migrationBuilder.CreateIndex(
                name: "IX_Blueprints_ItemId",
                table: "Blueprints",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Craftings_BlueprintId",
                table: "Craftings",
                column: "BlueprintId");

            migrationBuilder.CreateIndex(
                name: "IX_Enemies_FloorItemId",
                table: "Enemies",
                column: "FloorItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FloorItems_FloorId",
                table: "FloorItems",
                column: "FloorId");

            migrationBuilder.CreateIndex(
                name: "IX_FloorItems_ItemInstanceId",
                table: "FloorItems",
                column: "ItemInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Floors_BuildingId",
                table: "Floors",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Chests_FloorItemId",
                table: "Chests",
                column: "FloorItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ingrediences_RecipeId",
                table: "Ingrediences",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_ItemInstanceId",
                table: "InventoryItems",
                column: "ItemInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_PlayerId",
                table: "InventoryItems",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemInstances_ItemId",
                table: "ItemInstances",
                column: "ItemId");

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
                name: "IX_Players_FloorId",
                table: "Players",
                column: "FloorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlueprintPlayers");

            migrationBuilder.DropTable(
                name: "Craftings");

            migrationBuilder.DropTable(
                name: "Enemies");

            migrationBuilder.DropTable(
                name: "Chests");

            migrationBuilder.DropTable(
                name: "Ingrediences");

            migrationBuilder.DropTable(
                name: "InventoryItems");

            migrationBuilder.DropTable(
                name: "MineBlocks");

            migrationBuilder.DropTable(
                name: "RecipeTimes");

            migrationBuilder.DropTable(
                name: "Blueprints");

            migrationBuilder.DropTable(
                name: "FloorItems");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Blocks");

            migrationBuilder.DropTable(
                name: "MineLayers");

            migrationBuilder.DropTable(
                name: "ItemInstances");

            migrationBuilder.DropTable(
                name: "Floors");

            migrationBuilder.DropTable(
                name: "Mines");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Buildings");
        }
    }
}
