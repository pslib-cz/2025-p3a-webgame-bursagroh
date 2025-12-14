using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace game.Server.Models
{
    public class Floor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FloorId { get; set; }
        public int BuildingId { get; set; }
        [ForeignKey("BuildingId")]
        [JsonIgnore]
        public Building? Building { get; set; }
        public int Level { get; set; }

        public ICollection<FloorItem>? FloorItems { get; set; }
    }
}
