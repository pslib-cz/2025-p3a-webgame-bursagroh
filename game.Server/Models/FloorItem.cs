using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace game.Server.Models
{
    public enum FloorItemType
    {
       Stair,
       Chest,
       Player
       
    }

    public class FloorItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FloorItemId { get; set; }

        public int FloorId { get; set; }
        [ForeignKey("FloorId")]
        [JsonIgnore]
        public Floor? Floor { get; set; }

        public int PositionX { get; set; }
        public int PositionY { get; set; }

        public FloorItemType FloorItemType { get; set; }
    }
}
