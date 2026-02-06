namespace game.Server.DTOs
{
    public class BuildingDto
    {
        public int BuildingId { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public string BuildingType { get; set; } = string.Empty;
        public int? Height { get; set; }
        public int? ReachedHeight { get; set; }
        public bool? IsBossDefeated { get; set; }
    }
}