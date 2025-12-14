using System.Text.Json.Serialization;

namespace game.Server.Models
{
    public enum Direction
    {
        ToPlayer,
        ToBank
    }

    public class MovePlayerMoneyRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Direction Direction { get; set; }
        public int Amount { get; set; }
    }
}
