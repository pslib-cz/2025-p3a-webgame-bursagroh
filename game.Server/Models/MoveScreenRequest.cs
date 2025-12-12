using System.Text.Json.Serialization;

namespace game.Server.Models
{
    public class MoveScreenRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ScreenTypes NewScreenType { get; set; }
    }
}
