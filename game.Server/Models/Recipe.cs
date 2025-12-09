using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace game.Server.Models
{
    public class Recipe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecipeId { get; set; }
        public string? Name { get; set; }
        public Guid PlayerId { get; set; }

        public Player Player { get; set; } = null!;
    }
}
