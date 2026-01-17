namespace game.Server.DTOs
{
    public class BlueprintDto
    {
        public int BlueprintId { get; set; }
        public int Price { get; set; }
        public ItemDto Item { get; set; } = null!;
        public List<CraftingDto> Craftings { get; set; } = new();
    }
}
