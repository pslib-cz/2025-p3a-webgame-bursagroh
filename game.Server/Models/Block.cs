using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public BlockType BlockType { get; set; }
        public int ItemId { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; } = null!;

        public int changeOfGenerating = 1;

        public int MinAmount { get; set; }
        public int MaxAmount { get; set; }
        public ICollection<MineBlock> MineBlocks { get; set; } = new List<MineBlock>();
    }
}