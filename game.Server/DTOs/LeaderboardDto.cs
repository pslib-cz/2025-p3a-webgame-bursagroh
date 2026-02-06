using System.Text.Json.Serialization;

namespace game.Server.DTOs
{
    public class LeaderboardDto
    {
        public int RecipeId { get; set; }

        [JsonIgnore]
        public Guid PlayerId { get; set; }
        public PlayerNameDto Player { get; set; } = null!;
        public double Duration { get; set; }
    }
}
