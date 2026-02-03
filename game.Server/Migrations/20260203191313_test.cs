using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace game.Server.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxHealth",
                table: "Enemies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Blueprints",
                columns: new[] { "BlueprintId", "ItemId", "Price" },
                values: new object[,]
                {
                    { 22, 40, 500 },
                    { 23, 41, 500 },
                    { 24, 42, 500 }
                });

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 10,
                column: "Damage",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 11,
                column: "Damage",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 12,
                column: "Damage",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 13,
                column: "Damage",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 14,
                column: "Damage",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 15,
                column: "Damage",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 16,
                column: "Damage",
                value: 7);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Blueprints",
                keyColumn: "BlueprintId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Blueprints",
                keyColumn: "BlueprintId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Blueprints",
                keyColumn: "BlueprintId",
                keyValue: 24);

            migrationBuilder.DropColumn(
                name: "MaxHealth",
                table: "Enemies");

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 10,
                column: "Damage",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 11,
                column: "Damage",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 12,
                column: "Damage",
                value: 30);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 13,
                column: "Damage",
                value: 40);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 14,
                column: "Damage",
                value: 50);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 15,
                column: "Damage",
                value: 60);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 16,
                column: "Damage",
                value: 70);
        }
    }
}
