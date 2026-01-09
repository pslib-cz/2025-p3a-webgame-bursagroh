using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace game.Server.Migrations
{
    /// <inheritdoc />
    public partial class test1111 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_ItemInstances_ItemInstanceId",
                table: "InventoryItems");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Players_PlayerId",
                table: "InventoryItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryItems",
                table: "InventoryItems");

            migrationBuilder.RenameTable(
                name: "InventoryItems",
                newName: "InventoryItem");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryItems_PlayerId",
                table: "InventoryItem",
                newName: "IX_InventoryItem_PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryItems_ItemInstanceId",
                table: "InventoryItem",
                newName: "IX_InventoryItem_ItemInstanceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryItem",
                table: "InventoryItem",
                column: "InventoryItemId");

            migrationBuilder.CreateTable(
                name: "ItemMineBlock",
                columns: table => new
                {
                    ItemInstanceId = table.Column<int>(type: "INTEGER", nullable: false),
                    MineBlockId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemMineBlock", x => new { x.ItemInstanceId, x.MineBlockId });
                    table.ForeignKey(
                        name: "FK_ItemMineBlock_ItemInstances_ItemInstanceId",
                        column: x => x.ItemInstanceId,
                        principalTable: "ItemInstances",
                        principalColumn: "ItemInstanceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemMineBlock_MineBlocks_MineBlockId",
                        column: x => x.MineBlockId,
                        principalTable: "MineBlocks",
                        principalColumn: "MineBlockId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemMineBlock_MineBlockId",
                table: "ItemMineBlock",
                column: "MineBlockId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItem_ItemInstances_ItemInstanceId",
                table: "InventoryItem",
                column: "ItemInstanceId",
                principalTable: "ItemInstances",
                principalColumn: "ItemInstanceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItem_Players_PlayerId",
                table: "InventoryItem",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItem_ItemInstances_ItemInstanceId",
                table: "InventoryItem");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItem_Players_PlayerId",
                table: "InventoryItem");

            migrationBuilder.DropTable(
                name: "ItemMineBlock");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryItem",
                table: "InventoryItem");

            migrationBuilder.RenameTable(
                name: "InventoryItem",
                newName: "InventoryItems");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryItem_PlayerId",
                table: "InventoryItems",
                newName: "IX_InventoryItems_PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryItem_ItemInstanceId",
                table: "InventoryItems",
                newName: "IX_InventoryItems_ItemInstanceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryItems",
                table: "InventoryItems",
                column: "InventoryItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_ItemInstances_ItemInstanceId",
                table: "InventoryItems",
                column: "ItemInstanceId",
                principalTable: "ItemInstances",
                principalColumn: "ItemInstanceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_Players_PlayerId",
                table: "InventoryItems",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
