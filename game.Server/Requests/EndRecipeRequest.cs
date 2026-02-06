using game.Server.Models;
using System.Text.Json.Serialization;
using game.Server.Types;

public class AssembledIngredience
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public IngredienceTypes Type { get; set; }
}

public class EndRecipeRequest
{
    public Guid PlayerId { get; set; }
    public List<AssembledIngredience> PlayerAssembly { get; set; } = new List<AssembledIngredience>();
}