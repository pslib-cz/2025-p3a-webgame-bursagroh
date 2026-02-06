namespace game.Server.DTOs
{
    public class FloorItemDto
    {
        public int FloorItemId { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public ChestDto? Chest { get; set; }
        public EnemyDto? Enemy { get; set; }
        public ItemInstanceDto? ItemInstance { get; set; }
        public string FloorItemType { get; set; } = string.Empty;
    }
}
