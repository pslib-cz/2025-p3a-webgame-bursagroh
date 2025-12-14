namespace game.Server.Models
{
    public class BlueprintPlayer
    {
        public Guid PlayerId { get; set; }
        public int BlueprintId { get; set; }

        public Player Player { get; set; } = null!;
        public Blueprint Blueprint { get; set; } = null!;
    }
}