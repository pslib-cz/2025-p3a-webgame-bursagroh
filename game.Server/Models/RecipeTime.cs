using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace game.Server.Models
{
    public class RecipeTime
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecipeId { get; set; }
        public Guid PlayerId { get; set; }
        public DateTime Time { get; set; }

        public Player Player { get; set; } = null!;
    }
}
