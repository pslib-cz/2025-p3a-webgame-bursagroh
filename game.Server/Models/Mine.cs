using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace game.Server.Models
{
    public class Mine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MineId { get; set; }
        public Guid PlayerId { get; set; }

        public ICollection<MineLayer> MineLayers { get; set; } = new List<MineLayer>();
    }
}
