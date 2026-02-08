using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using game.Server.Types;

namespace game.Server.Models
{
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
        public int? ReachedHeight { get; set; } = null;
        public bool? IsBossDefeated { get; set; } = false;

        public ICollection<Floor>? Floors { get; set; }
    }
}
