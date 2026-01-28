namespace game.Server.DTOs
{
    public class InventoryItemDto
    {
        public int InventoryItemId { get; set; }
        public ItemInstanceDto? ItemInstance { get; set; }
    }
}