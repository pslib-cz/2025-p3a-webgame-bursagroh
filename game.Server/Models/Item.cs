using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using game.Server.Types;

namespace game.Server.Models
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }
        public string Name { get; set; } = String.Empty;
        public string? Description { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ItemTypes ItemType { get; set; }
        public int Weight { get; set; }
        public int Damage { get; set; }
        public int MaxDurability { get; set; }
        public int ChangeOfGenerating { get; set; }

    }
}
