namespace game.Server.DTOs
{
    public class FloorDto
    {
        public int FloorId { get; set; }
        public int BuildingId { get; set; }
        public int Level { get; set; }
        public List<FloorItemDto> FloorItems { get; set; } = new();
    }
}
