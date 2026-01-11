using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace game.Server.Migrations
{
    /// <inheritdoc />
    public partial class idk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 39,
                column: "MaxDurability",
                value: 15);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 39,
                column: "MaxDurability",
                value: 5);
        }
    }
}
