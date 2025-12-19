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
        Light
    }

    public class Player
    {
        [Key]
        public Guid PlayerId { get; set; } = new Guid("4b1e8a93-7d92-4f7f-80c1-525c345b85e0");
        public string Name { get; set; } = String.Empty;
        public int Money { get; set; }
        [JsonIgnore]
        public int BankBalance { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ScreenTypes ScreenType { get; set; } = ScreenTypes.City;
        //public int? BuildingId { get; set; }
        //[ForeignKey("BuildingId")]
        //public Building? Building { get; set; }
        //public int? FloorItemId { get; set; }
        //[ForeignKey("FloorItemId")]
        //public FloorItem? FloorItem { get; set; }

        public int PositionX { get; set; }
        public int PositionY { get; set; }

        public int SubPositionX { get; set; }
        public int SubPositionY { get; set; }
        public int? FloorId { get; set; } = null!;

        public int Capacity { get; set; } = 10;
        public int Seed { get; set; } = 0;
    }
}