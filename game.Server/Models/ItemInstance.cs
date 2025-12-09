using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace game.Server.Models
{
    public class ItemInstance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemInstanceId { get; set; }

        public int ItemId { get; set; }
        public int Durability { get; set; }

        public ICollection<Item>? Items { get; set; }
    }
}
