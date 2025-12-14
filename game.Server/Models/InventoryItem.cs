using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace game.Server.Models
{
    public class InventoryItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InventoryItemId { get; set; }

        public Guid PlayerId { get; set; }

        public int ItemInstanceId { get; set; }

        public bool IsInBank { get; set; } = false;
    }
}
