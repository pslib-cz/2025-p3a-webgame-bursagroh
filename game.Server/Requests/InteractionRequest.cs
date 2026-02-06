using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace game.Server.Requests
{
    public class InteractionRequest
    {
        public int TargetX { get; set; }

        public int TargetY { get; set; }

        public int? InventoryItemId { get; set; }
    }
}
