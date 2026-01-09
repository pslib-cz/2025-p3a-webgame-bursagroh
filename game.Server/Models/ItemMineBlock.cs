using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace game.Server.Models
{
    public class ItemMineBlock
    {
        public int ItemInstanceId { get; set; }

        public int MineBlockId { get; set; }

        [ForeignKey("ItemInstanceId")]
        public virtual ItemInstance ItemInstance { get; set; } = null!;

        [ForeignKey("MineBlockId")]
        public virtual MineBlock MineBlock { get; set; } = null!;
    }
}