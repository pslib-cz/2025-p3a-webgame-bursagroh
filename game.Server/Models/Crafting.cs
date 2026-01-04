using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace game.Server.Models
{
    public class Crafting
    {
        [Key]
        public int CraftingId { get; set; }
        public int BlueprintId { get; set; }
        public int ItemId { get; set; }
        public int Amount { get; set; }

        [ForeignKey("BlueprintId")]
        [JsonIgnore]
        public Blueprint? Blueprint { get; set; }


    }
}
