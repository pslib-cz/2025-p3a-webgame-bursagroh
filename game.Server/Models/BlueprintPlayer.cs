using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace game.Server.Models
{
    public class BlueprintPlayer
    {
        public Guid PlayerId { get; set; }
        public int BlueprintId { get; set; }

        [ForeignKey("PlayerId")]
        public Player Player { get; set; } = null!;

        [ForeignKey("BlueprintId")]
        public Blueprint Blueprint { get; set; } = null!;
    }
}