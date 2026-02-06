namespace game.Server.DTOs
{
    public class CraftingDto
    {
        public CraftingItemDto Item { get; set; } = null!;
        public int Amount { get; set; }
    }
}
