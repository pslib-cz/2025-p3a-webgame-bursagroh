namespace game.Server.DTOs
{
    public class MineBlockDto
    {
        public int MineBlockId { get; set; }
        public int MineLayerId { get; set; }
        public int Index { get; set; }
        public MineBlockDetailsDto Block { get; set; } = null!;
        public int Health { get; set; }
    }
}
