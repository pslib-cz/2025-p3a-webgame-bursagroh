namespace game.Server.DTOs
{
    public class MineItemDto
    {
        public int FloorItemId { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public ItemInstanceDto? ItemInstance { get; set; }
    }
}