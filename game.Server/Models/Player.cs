using game.Server.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using game.Server.Types;

public class Player
{
    [Key]
    public Guid PlayerId { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = String.Empty;
    public int Money { get; set; } = 0;
    public int BankBalance { get; set; } = 0;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ScreenTypes ScreenType { get; set; } = ScreenTypes.Fountain;
    public int PositionX { get; set; } = 0;
    public int PositionY { get; set; } = 0;
    public int SubPositionX { get; set; } = 0;
    public int SubPositionY { get; set; } = 0;

    public DateTime LastModified { get; set; }
    public int? FloorId { get; set; }
    [ForeignKey("FloorId")]
    public Floor? Floor { get; set; }
    public int Capacity { get; set; } = 10;
    public int Seed { get; set; } = new Random().Next();
    public int Health { get; set; } = 20;
    public int MaxHealth { get; set; } = 20;

    public int MineId { get; set; }

    [JsonIgnore]
    public virtual ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();

    public int? ActiveInventoryItemId { get; set; }
    [ForeignKey("ActiveInventoryItemId")]
    public virtual InventoryItem? ActiveInventoryItem { get; set; }
}