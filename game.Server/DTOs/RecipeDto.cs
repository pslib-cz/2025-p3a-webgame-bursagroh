namespace game.Server.DTOs
{
    public class RecipeDto
    {
        public int RecipeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<RecipeIngredienceDto> Ingrediences { get; set; } = new();
    }
}
