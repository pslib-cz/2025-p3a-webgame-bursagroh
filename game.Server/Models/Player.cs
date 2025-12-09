using System.ComponentModel.DataAnnotations;

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
        public int BankBalance { get; set; }
        public ScreenTypes ScreenType { get; set; } = ScreenTypes.City;
        public int? BuildingID { get; set; }
        public int? FloorItemID { get; set; }
        public int Capacity { get; set; } = 10;
        public int Seed { get; set; } = 252;
    }
}