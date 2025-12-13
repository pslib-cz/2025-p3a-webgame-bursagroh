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

        public ICollection<BlueprintPlayer> BlueprintPlayers { get; set; } = new List<BlueprintPlayer>();
    }
}
