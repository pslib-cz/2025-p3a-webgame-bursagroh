using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace game.Server.Migrations
{
    /// <inheritdoc />
    public partial class hovno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemId", "ChangeOfGenerating", "Damage", "Description", "ItemType", "MaxDurability", "Name", "Weight" },
                values: new object[] { 100, 0, 100, "Drop this into fountain to win", 0, 1000000, "Mythical Sword", 1 });

            migrationBuilder.InsertData(
                table: "Blueprints",
                columns: new[] { "BlueprintId", "ItemId", "Price" },
                values: new object[] { 25, 100, 10000 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Blueprints",
                keyColumn: "BlueprintId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 100);
        }
    }
}
