using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace game.Server.Migrations
{
    /// <inheritdoc />
    public partial class testicek : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 1,
                column: "Name",
                value: "Rock");

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemId", "Damage", "Description", "ItemInstanceId", "ItemType", "MaxDurability", "Name", "Weight" },
                values: new object[,]
                {
                    { 3, 0, null, null, 0, 0, "Copper Ore", 0 },
                    { 4, 0, null, null, 0, 0, "Silver Ore", 0 },
                    { 5, 0, null, null, 0, 0, "Gold Ore", 0 },
                    { 6, 0, null, null, 0, 0, "Unobtainium Ore", 0 }
                });

            migrationBuilder.InsertData(
                table: "Blocks",
                columns: new[] { "BlockId", "BlockType", "ItemId", "MaxAmount", "MinAmount" },
                values: new object[,]
                {
                    { 3, 4, 3, 1, 1 },
                    { 4, 5, 4, 1, 1 },
                    { 5, 6, 5, 1, 1 },
                    { 6, 7, 6, 1, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Blocks",
                keyColumn: "BlockId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Blocks",
                keyColumn: "BlockId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Blocks",
                keyColumn: "BlockId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Blocks",
                keyColumn: "BlockId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 1,
                column: "Name",
                value: "Stone");
        }
    }
}
