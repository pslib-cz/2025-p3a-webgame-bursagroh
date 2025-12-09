using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace game.Server.Models
{
    public enum ItemTypes
    {
        Sword,
        Axe,
        Healing_Potion
    }

    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }
        public string Name { get; set; } = String.Empty;
        public string? Description { get; set; }
        public ItemTypes ItemType { get; set; }
        public int Weight { get; set; }
        public int Damage { get; set; }
        public int MaxDurability { get; set; }
        public ICollection<Item>? Items { get; set; }
    }
}
