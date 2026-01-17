namespace game.Server.DTOs
{
    public class BankInventoryDto
    {
        public int InventoryItemId { get; set; }
        public ItemInstanceDto ItemInstance { get; set; } = null!;
    }
}
