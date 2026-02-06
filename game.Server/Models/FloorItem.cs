using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using game.Server.Types;

namespace game.Server.Models
{
    

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

        public Chest? Chest { get; set; }
        public Enemy? Enemy { get; set; }

        public int? ItemInstanceId { get; set; }
        [ForeignKey("ItemInstanceId")]
        public ItemInstance? ItemInstance { get; set; }


        [JsonConverter(typeof(JsonStringEnumConverter))]
        public FloorItemType FloorItemType { get; set; }
    }
}
