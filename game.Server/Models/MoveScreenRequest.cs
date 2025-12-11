using System.Text.Json.Serialization;

namespace game.Server.Models
{
    public class MoveScreenRequest
    {
        public ScreenTypes NewScreenType { get; set; }
    }
}
