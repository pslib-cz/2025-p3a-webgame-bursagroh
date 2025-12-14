using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace game.Server.Models
{
    public class MineBlock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MineBlockId { get; set; }

        public int MineLayerId { get; set; }
        [ForeignKey("MineLayerId")]
        [JsonIgnore]
        public MineLayer? MineLayer { get; set; }

        public int Index { get; set; }

        public int BlockId { get; set; }

        [ForeignKey("BlockId")]
        public Block Block { get; set; } = null!; 
    }
}