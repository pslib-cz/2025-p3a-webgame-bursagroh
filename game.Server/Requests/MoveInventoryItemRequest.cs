namespace game.Server.Requests
{
    public class MoveInventoryItemRequest
    {
        public List<int> InventoryItemIds { get; set; } = new();
    }
}