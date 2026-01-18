using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace game.Server.Models
{
    public class MineInteractionRequest
    {
        public int TargetX { get; set; }

        public int TargetY { get; set; }

    }
}
