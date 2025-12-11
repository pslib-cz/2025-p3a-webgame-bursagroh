using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace game.Server.Models
{
    // Placeholder for the BlockType enum
    public enum BlockType
    {
        Ore,
        Stone,
        Wood
    }

    // The Block entity
    public class Block
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BlockId { get; set; }

        public BlockType BlockType { get; set; }
        public int ItemId { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; } = null!;

        public int MinAmount { get; set; }
        public int MaxAmount { get; set; }
        public ICollection<MineBlock> MineBlocks { get; set; } = new List<MineBlock>();
    }
}