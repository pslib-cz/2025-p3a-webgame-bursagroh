using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace game.Server.Models
{
    public enum EnemyType
    {
        Zombie,
        Skeleton,
        Dragon
    }

    public class Enemy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EnemyId { get; set; }
        public int Health { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EnemyType EnemyType { get; set; }
        public int FloorItemId { get; set; }
        public int? ItemInstanceId { get; set; }

        public virtual ItemInstance? ItemInstance { get; set; }
    }
}
