using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace game.Server.Migrations
{
    /// <inheritdoc />
    public partial class m3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 1,
                column: "ChangeOfGenerating",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 2,
                column: "ChangeOfGenerating",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 3,
                column: "ChangeOfGenerating",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 4,
                column: "ChangeOfGenerating",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 5,
                column: "ChangeOfGenerating",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 6,
                column: "ChangeOfGenerating",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 7,
                column: "ChangeOfGenerating",
                value: 50);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 1,
                column: "ChangeOfGenerating",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 2,
                column: "ChangeOfGenerating",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 3,
                column: "ChangeOfGenerating",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 4,
                column: "ChangeOfGenerating",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 5,
                column: "ChangeOfGenerating",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 6,
                column: "ChangeOfGenerating",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 7,
                column: "ChangeOfGenerating",
                value: 0);
        }
    }
}
