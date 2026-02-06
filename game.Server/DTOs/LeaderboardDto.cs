namespace game.Server.DTOs
{
    public class LeaderboardDto
    {
        public int RecipeId { get; set; }
        public Guid PlayerId { get; set; }
        public PlayerNameDto Player { get; set; } = null!;
        public double Duration { get; set; }
    }
}
