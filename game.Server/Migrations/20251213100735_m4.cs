using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace game.Server.Migrations
{
    /// <inheritdoc />
    public partial class m4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 1,
                column: "ChangeOfGenerating",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 2,
                column: "ChangeOfGenerating",
                value: 80);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 3,
                column: "ChangeOfGenerating",
                value: 15);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 4,
                column: "ChangeOfGenerating",
                value: 15);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 6,
                column: "ChangeOfGenerating",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 7,
                column: "ChangeOfGenerating",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 1,
                column: "Name",
                value: "Classic Hamburger");

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "RecipeId", "Name" },
                values: new object[,]
                {
                    { 2, "Cheeseburger" },
                    { 3, "Bacon Burger" },
                    { 4, "Veggie Deluxe" },
                    { 5, "BLT Sandwich" },
                    { 6, "Chicken Sandwich" },
                    { 7, "The Works Burger" },
                    { 8, "Simple Salad Burger" },
                    { 9, "Triple Meat Stack" },
                    { 10, "Saucy Bacon Tomato Burger" },
                    { 11, "Bacon Cheeseburger Deluxe" }
                });

            migrationBuilder.InsertData(
                table: "Ingrediences",
                columns: new[] { "IngredienceId", "IngredienceType", "Order", "RecipeId" },
                values: new object[,]
                {
                    { 4, 2, 1, 2 },
                    { 5, 0, 2, 2 },
                    { 6, 7, 3, 2 },
                    { 7, 3, 4, 2 },
                    { 8, 2, 1, 3 },
                    { 9, 0, 2, 3 },
                    { 10, 6, 3, 3 },
                    { 11, 3, 4, 3 },
                    { 12, 2, 1, 4 },
                    { 13, 1, 2, 4 },
                    { 14, 4, 3, 4 },
                    { 15, 5, 4, 4 },
                    { 16, 1, 5, 4 },
                    { 17, 3, 6, 4 },
                    { 18, 2, 1, 5 },
                    { 19, 6, 2, 5 },
                    { 20, 1, 3, 5 },
                    { 21, 4, 4, 5 },
                    { 22, 3, 5, 5 },
                    { 23, 2, 1, 6 },
                    { 24, 5, 2, 6 },
                    { 25, 0, 3, 6 },
                    { 26, 1, 4, 6 },
                    { 27, 3, 5, 6 },
                    { 28, 2, 1, 7 },
                    { 29, 5, 2, 7 },
                    { 30, 4, 3, 7 },
                    { 31, 0, 4, 7 },
                    { 32, 6, 5, 7 },
                    { 33, 1, 6, 7 },
                    { 34, 3, 7, 7 },
                    { 35, 2, 1, 8 },
                    { 36, 1, 2, 8 },
                    { 37, 0, 3, 8 },
                    { 38, 3, 4, 8 },
                    { 39, 2, 1, 9 },
                    { 40, 0, 2, 9 },
                    { 41, 0, 3, 9 },
                    { 42, 0, 4, 9 },
                    { 43, 3, 5, 9 },
                    { 44, 2, 1, 10 },
                    { 45, 5, 2, 10 },
                    { 46, 0, 3, 10 },
                    { 47, 6, 4, 10 },
                    { 48, 4, 5, 10 },
                    { 49, 3, 6, 10 },
                    { 50, 2, 1, 11 },
                    { 51, 1, 2, 11 },
                    { 52, 4, 3, 11 },
                    { 53, 7, 4, 11 },
                    { 54, 0, 5, 11 },
                    { 55, 6, 6, 11 },
                    { 56, 3, 7, 11 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Ingrediences",
                keyColumn: "IngredienceId",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 11);

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
                keyValue: 6,
                column: "ChangeOfGenerating",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 7,
                column: "ChangeOfGenerating",
                value: 50);

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 1,
                column: "Name",
                value: "Hamburger");
        }
    }
}
