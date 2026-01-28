namespace game.Server.DTOs
{
    public class PlayerDto
    {
        public Guid PlayerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Money { get; set; }
        public int BankBalance { get; set; }
        public string ScreenType { get; set; } = string.Empty;
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public int SubPositionX { get; set; }
        public int SubPositionY { get; set; }
        public int? FloorId { get; set; }
        public int? MineId { get; set; }
        public int? ActiveInventoryItemId { get; set; }
        public int Capacity { get; set; }

        public int Health { get; set; }

    }
}