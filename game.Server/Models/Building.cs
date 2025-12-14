using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace game.Server.Models
{
    public enum BuildingTypes
    {
        Fountain,
        City,
        Bank,
        Restaurant,
        Mine,
        Blacksmith,
        Abandoned,
        AbandonedTrap,
        Road,
        RoadVertical,
        RoadHorizontal
    }

    public class Building
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BuildingId { get; set; }
        public Guid PlayerId { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BuildingTypes BuildingType { get; set; }
        public int? Height { get; set; }
        public int? ReachedHeight { get; set; }
        public bool? IsBossDefeated { get; set; } = false;

        public ICollection<Floor>? Floors { get; set; }
    }
}
