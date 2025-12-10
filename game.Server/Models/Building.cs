using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace game.Server.Models
{
    public enum BuildingTypes
    {
        Fountain,
        Road,
        Bank,
        Restaurant,
        Mine,
        Blacksmith,
        Abandoned,
        AbandonedTrap
    }

    public class Building
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BuildingId { get; set; }
        public Guid PlayerId { get; set; }
        public BuildingTypes BuildingType { get; set; }
        public int? Height { get; set; }
        public int? ReachedHeight { get; set; }
        public bool? IsBossDefeated { get; set; } = false;

        public Player Player = null!;
    }
}
