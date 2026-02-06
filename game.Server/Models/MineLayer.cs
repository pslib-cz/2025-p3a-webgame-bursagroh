using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace game.Server.Models
{
    public class MineLayer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MineLayerID { get; set; }

        public int MineId { get; set; }

        public int Depth { get; set; }
        public ICollection<MineBlock> MineBlocks { get; set; } = new List<MineBlock>();
    }
}