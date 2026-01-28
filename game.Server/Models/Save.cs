using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Save
{
    [Key]
    public Guid PlayerId { get; set; }
    public string SaveString { get; set; } = string.Empty;
}