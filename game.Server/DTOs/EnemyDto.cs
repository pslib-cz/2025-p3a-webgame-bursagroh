namespace game.Server.DTOs
{
    public class EnemyDto
    {
        public int EnemyId { get; set; }
        public int Health { get; set; }
        public string EnemyType { get; set; } = string.Empty;
    }
}
