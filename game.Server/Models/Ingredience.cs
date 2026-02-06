using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using game.Server.Types;

namespace game.Server.Models
{
    public class Ingredience
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IngredienceId { get; set; }
        public int RecipeId { get; set; }
        public int Order { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public IngredienceTypes IngredienceType { get; set; }
    }
}
