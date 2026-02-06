using System.ComponentModel.DataAnnotations;

namespace game.Server.Models
{
    public class ChestItem
    {
        [Key]
        public int ChestItemId { get; set; }
        public int ChestId { get; set; }

        public int ItemInstanceId { get; set; }
        public int Amount { get; set; }
    }
}
