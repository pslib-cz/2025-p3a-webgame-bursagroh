using System.Text.Json.Serialization;

namespace game.Server.DTOs
{
    public class ItemInstanceDto
    {
        public int ItemInstanceId { get; set; }
        public int Durability { get; set; }
        public ItemDto Item { get; set; } = null!;
    }
}
