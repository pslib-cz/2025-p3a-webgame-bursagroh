using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace game.Server.Models
{
    public enum ScreenTypes
    {
        City,
        Bank,
        Mine,
        Restaurant,
        Blacksmith,
        Floor,
        Fountain,
        Fight
    }

    public class Player
    {
        [Key]
        public Guid PlayerId { get; set; } = new Guid("4b1e8a93-7d92-4f7f-80c1-525c345b85e0");
        public string Name { get; set; } = String.Empty;
        public int Money { get; set; }
        public int BankBalance { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ScreenTypes ScreenType { get; set; } = ScreenTypes.City;

        public int PositionX { get; set; }
        public int PositionY { get; set; }

        public int SubPositionX { get; set; }
        public int SubPositionY { get; set; }
        public int? FloorId { get; set; }

        [ForeignKey("FloorId")]
        public Floor? Floor { get; set; }

        public int Capacity { get; set; } = 20;
        public int Seed { get; set; } = 0;

        public int Health { get; set; } = 100;

            [JsonIgnore]
        public virtual ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();

        public int? ActiveInventoryItemId { get; set; }

        [ForeignKey("ActiveInventoryItemId")]
        public virtual InventoryItem? ActiveInventoryItem { get; set; }
    }
}