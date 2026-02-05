using game.Server.Models;
using System.Text.Json.Serialization;

namespace game.Server.Requests
{
    public class MoveScreenRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ScreenTypes NewScreenType { get; set; }
    }
}
