using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace game.Server.Migrations
{
    /// <inheritdoc />
    public partial class test5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Blueprints_ItemId",
                table: "Blueprints",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blueprints_Items_ItemId",
                table: "Blueprints",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blueprints_Items_ItemId",
                table: "Blueprints");

            migrationBuilder.DropIndex(
                name: "IX_Blueprints_ItemId",
                table: "Blueprints");
        }
    }
}
