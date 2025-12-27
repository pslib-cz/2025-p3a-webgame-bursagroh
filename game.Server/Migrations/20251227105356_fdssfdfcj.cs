using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace game.Server.Migrations
{
    /// <inheritdoc />
    public partial class fdssfdfcj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FloorItems_ItemInstances_ItemInstanceId",
                table: "FloorItems");

            migrationBuilder.AddForeignKey(
                name: "FK_FloorItems_ItemInstances_ItemInstanceId",
                table: "FloorItems",
                column: "ItemInstanceId",
                principalTable: "ItemInstances",
                principalColumn: "ItemInstanceId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FloorItems_ItemInstances_ItemInstanceId",
                table: "FloorItems");

            migrationBuilder.AddForeignKey(
                name: "FK_FloorItems_ItemInstances_ItemInstanceId",
                table: "FloorItems",
                column: "ItemInstanceId",
                principalTable: "ItemInstances",
                principalColumn: "ItemInstanceId");
        }
    }
}
