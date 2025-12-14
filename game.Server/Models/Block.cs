using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace game.Server.Models
{
    public enum BlockType
    {
        Empty,
        Rock,
        Wooden_Frame,
        Iron_Ore,
        Copper_Ore,
        Silver_Ore,
        Gold_Ore,
        Unobtanium_Ore

    }

    public class Block
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BlockId { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BlockType BlockType { get; set; }
        public int ItemId { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; } = null!;

        public int MinAmount { get; set; }
        public int MaxAmount { get; set; }
    }
}