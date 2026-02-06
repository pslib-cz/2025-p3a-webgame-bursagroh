using System.Text.Json.Serialization;
using game.Server.Types;

namespace game.Server.Requests
{
    public class MoveScreenRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ScreenTypes NewScreenType { get; set; }
    }
}
