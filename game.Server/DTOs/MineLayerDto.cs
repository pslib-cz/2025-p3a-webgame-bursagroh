namespace game.Server.DTOs
{
    public class MineLayerDto
    {
        public int MineLayerId { get; set; }
        public int Depth { get; set; }
        public List<MineBlockDto> MineBlocks { get; set; } = new();
    }
}
