using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace game.Server.Requests
{
    public class MineInteractionRequest
    {
        public int TargetX { get; set; }

        public int TargetY { get; set; }

    }
}
