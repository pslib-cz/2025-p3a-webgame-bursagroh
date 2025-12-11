using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace game.Server.Models
{
    public class MineBlock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MineBlockId { get; set; }
        public int MineLayerId { get; set; }

        [ForeignKey("MineLayerId")]
        public MineLayer MineLayer { get; set; } = null!;

        public int Index { get; set; }

        public int BlockId { get; set; }

        [ForeignKey("BlockId")]
        public Block Block { get; set; } = null!;
    }
}