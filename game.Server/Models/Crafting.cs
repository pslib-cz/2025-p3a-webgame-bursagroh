using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace game.Server.Models
{
    public class Crafting
    {
        [Key]
        public int CraftingId { get; set; }
        public int BlueprintId { get; set; }
        public int ItemId { get; set; }
        public int Amount { get; set; }


    }
}
