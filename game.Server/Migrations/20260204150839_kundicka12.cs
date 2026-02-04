using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace game.Server.Migrations
{
    /// <inheritdoc />
    public partial class kundicka12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxHealth",
                table: "Enemies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxHealth",
                table: "Enemies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
