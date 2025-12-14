using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace game.Server.Models
{
    public class Chest
    {
        [Key]
        public int ChestId { get; set; }
        public int FloorItemId { get; set; }
    }
}
