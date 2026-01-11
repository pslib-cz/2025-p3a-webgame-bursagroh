using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace game.Server.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Blueprints",
                columns: new[] { "BlueprintId", "ItemId", "Price" },
                values: new object[] { 2, 30, 5 });

            migrationBuilder.UpdateData(
                table: "Craftings",
                keyColumn: "CraftingId",
                keyValue: 1,
                column: "Amount",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 39,
                column: "MaxDurability",
                value: 15);

            migrationBuilder.InsertData(
                table: "Craftings",
                columns: new[] { "CraftingId", "Amount", "BlueprintId", "ItemId" },
                values: new object[] { 2, 3, 2, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Craftings_ItemId",
                table: "Craftings",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Craftings_Items_ItemId",
                table: "Craftings",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Craftings_Items_ItemId",
                table: "Craftings");

            migrationBuilder.DropIndex(
                name: "IX_Craftings_ItemId",
                table: "Craftings");

            migrationBuilder.DeleteData(
                table: "Craftings",
                keyColumn: "CraftingId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Blueprints",
                keyColumn: "BlueprintId",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Craftings",
                keyColumn: "CraftingId",
                keyValue: 1,
                column: "Amount",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 39,
                column: "MaxDurability",
                value: 5);
        }
    }
}
