namespace game.Server.DTOs
{
    public class ItemDto
    {
        public int ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string ItemType { get; set; } = string.Empty;
        public int Weight { get; set; }
        public int Damage { get; set; }
        public int MaxDurability { get; set; }
    }
}
