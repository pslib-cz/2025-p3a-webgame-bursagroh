using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace game.Server.Migrations
{
    /// <inheritdoc />
    public partial class m2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChangeOfGenerating",
                table: "Items",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Blocks",
                keyColumn: "BlockId",
                keyValue: 1,
                columns: new[] { "BlockType", "MaxAmount" },
                values: new object[] { 2, 1 });

            migrationBuilder.UpdateData(
                table: "Blocks",
                keyColumn: "BlockId",
                keyValue: 2,
                column: "BlockType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Blocks",
                keyColumn: "BlockId",
                keyValue: 4,
                column: "BlockType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Blocks",
                keyColumn: "BlockId",
                keyValue: 5,
                column: "BlockType",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Blocks",
                keyColumn: "BlockId",
                keyValue: 6,
                column: "BlockType",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 1,
                columns: new[] { "ChangeOfGenerating", "Description", "ItemType", "Name" },
                values: new object[] { 0, "Wooden Frame", 4, "Wooden Frame" });

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 2,
                columns: new[] { "ChangeOfGenerating", "Description", "ItemType", "Name" },
                values: new object[] { 0, "Rock", 4, "Rock" });

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 3,
                columns: new[] { "ChangeOfGenerating", "Description", "ItemType" },
                values: new object[] { 0, "Copper Ore", 4 });

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 4,
                columns: new[] { "ChangeOfGenerating", "Description", "ItemType", "Name" },
                values: new object[] { 0, "Iron Ore", 4, "Iron Ore" });

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 5,
                columns: new[] { "ChangeOfGenerating", "Description", "ItemType", "Name" },
                values: new object[] { 0, "Silver Ore", 4, "Silver Ore" });

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 6,
                columns: new[] { "ChangeOfGenerating", "Description", "ItemType", "Name" },
                values: new object[] { 0, "Gold Ore", 4, "Gold Ore" });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemId", "ChangeOfGenerating", "Damage", "Description", "ItemInstanceId", "ItemType", "MaxDurability", "Name", "Weight" },
                values: new object[,]
                {
                    { 7, 0, 0, "Unobtainium Ore", null, 4, 0, "Unobtainium Ore", 0 },
                    { 10, 0, 1, "Wooden Sword", null, 0, 20, "Wooden Sword", 1 },
                    { 11, 0, 2, "Rock Sword", null, 0, 40, "Rock Sword", 1 },
                    { 12, 0, 3, "Copper Sword", null, 0, 60, "Copper Sword", 1 },
                    { 13, 0, 4, "Iron Sword", null, 0, 80, "Iron Sword", 1 },
                    { 14, 0, 5, "Silver Sword", null, 0, 100, "Silver Sword", 1 },
                    { 15, 0, 6, "Gold Sword", null, 0, 120, "Gold Sword", 1 },
                    { 16, 0, 7, "Unobtainium Sword", null, 0, 240, "Unobtainium Sword", 1 },
                    { 20, 0, 1, "Wooden Axe", null, 1, 20, "Wooden Axe", 1 },
                    { 21, 0, 2, "Rock Axe", null, 1, 40, "Rock Axe", 1 },
                    { 22, 0, 3, "Copper Axe", null, 1, 60, "Copper Axe", 1 },
                    { 23, 0, 4, "Iron Axe", null, 1, 80, "Iron Axe", 1 },
                    { 24, 0, 5, "Silver Axe", null, 1, 100, "Silver Axe", 1 },
                    { 25, 0, 6, "Gold Axe", null, 1, 120, "Gold Axe", 1 },
                    { 26, 0, 7, "Unobtainium Axe", null, 1, 240, "Unobtainium Axe", 1 },
                    { 30, 0, 1, "Wooden Pickaxe", null, 2, 20, "Wooden Pickaxe", 1 },
                    { 31, 0, 2, "Rock Pickaxe", null, 2, 40, "Rock Pickaxe", 1 },
                    { 32, 0, 3, "Copper Pickaxe", null, 2, 60, "Copper Pickaxe", 1 },
                    { 33, 0, 4, "Iron Pickaxe", null, 2, 80, "Iron Pickaxe", 1 },
                    { 34, 0, 5, "Silver Pickaxe", null, 2, 100, "Silver Pickaxe", 1 },
                    { 35, 0, 6, "Gold Pickaxe", null, 2, 120, "Gold Pickaxe", 1 },
                    { 36, 0, 7, "Unobtainium Pickaxe", null, 2, 240, "Unobtainium Pickaxe", 1 }
                });

            migrationBuilder.InsertData(
                table: "Blocks",
                columns: new[] { "BlockId", "BlockType", "ItemId", "MaxAmount", "MinAmount" },
                values: new object[] { 7, 7, 7, 1, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Blocks",
                keyColumn: "BlockId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 7);

            migrationBuilder.DropColumn(
                name: "ChangeOfGenerating",
                table: "Items");

            migrationBuilder.UpdateData(
                table: "Blocks",
                keyColumn: "BlockId",
                keyValue: 1,
                columns: new[] { "BlockType", "MaxAmount" },
                values: new object[] { 1, 3 });

            migrationBuilder.UpdateData(
                table: "Blocks",
                keyColumn: "BlockId",
                keyValue: 2,
                column: "BlockType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Blocks",
                keyColumn: "BlockId",
                keyValue: 4,
                column: "BlockType",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Blocks",
                keyColumn: "BlockId",
                keyValue: 5,
                column: "BlockType",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Blocks",
                keyColumn: "BlockId",
                keyValue: 6,
                column: "BlockType",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 1,
                columns: new[] { "Description", "ItemType", "Name" },
                values: new object[] { null, 0, "Rock" });

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 2,
                columns: new[] { "Description", "ItemType", "Name" },
                values: new object[] { null, 0, "Iron Ore" });

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 3,
                columns: new[] { "Description", "ItemType" },
                values: new object[] { null, 0 });

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 4,
                columns: new[] { "Description", "ItemType", "Name" },
                values: new object[] { null, 0, "Silver Ore" });

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 5,
                columns: new[] { "Description", "ItemType", "Name" },
                values: new object[] { null, 0, "Gold Ore" });

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 6,
                columns: new[] { "Description", "ItemType", "Name" },
                values: new object[] { null, 0, "Unobtainium Ore" });
        }
    }
}
