using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using game.Server.Types;

namespace game.Server.Models
{
    public class Enemy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EnemyId { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EnemyType EnemyType { get; set; }
        public int FloorItemId { get; set; }
        public int? ItemInstanceId { get; set; }

        public virtual ItemInstance? ItemInstance { get; set; }
    }
}
