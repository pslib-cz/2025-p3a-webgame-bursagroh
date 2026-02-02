namespace game.Server.Models
{
    public class MoveInventoryItemRequest
    {
        public List<int> InventoryItemIds { get; set; } = new();
    }
}