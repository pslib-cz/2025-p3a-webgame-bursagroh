using System.ComponentModel.DataAnnotations;

namespace game.Server.Models
{
    public class Chest
    {
        [Key]
        public int ChestId { get; set; }
        public int FloorItemId { get; set; }
        public virtual ICollection<ItemInstance>? ItemInstances { get; set; }
    }
}