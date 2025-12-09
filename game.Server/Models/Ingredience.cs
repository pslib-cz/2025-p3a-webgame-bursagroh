using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace game.Server.Models
{
    public enum IngredienceTypes
    {
        Meat,
        Salad,
        Bun,
        Tomato,
        Sauce,
        Bacon
    }

    public class Ingredience
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IngredienceId { get; set; }
        public int RecipeId { get; set; }
        public int Order { get; set; }
        public IngredienceTypes IngredienceType { get; set; }   

        public Recipe Recipe { get; set; } = null!;
    }
}
