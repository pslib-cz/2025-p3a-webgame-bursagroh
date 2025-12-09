using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace game.Server.Models
{
    public class Blueprint
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BlueprintId { get; set; }
        public int ItemId { get; set; }
        public int Price { get; set; }

        public Item Item { get; set; } = null!;

        public ICollection<Crafting> Craftings { get; set; } = new List<Crafting>();
        public ICollection<BlueprintPlayer> PlayersBlueprints { get; set; } = new List<BlueprintPlayer>();
    }
}
