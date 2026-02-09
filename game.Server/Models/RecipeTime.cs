using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class RecipeTime
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RecipeTimeId { get; set; }

    public int RecipeId { get; set; }
    public Guid PlayerId { get; set; }

    public DateTime? StartTime { get; set; } = null;
    public DateTime? EndTime { get; set; } = null;

    // Add this attribute so EF Core doesn't try to save it as a column
    [NotMapped]
    public double Duration
    {
        get
        {
            if (StartTime.HasValue && EndTime.HasValue)
            {
                return (EndTime.Value - StartTime.Value).TotalSeconds;
            }
            return 0.0;
        }
    }

    // Make sure you have the navigation property for the leaderboard mapping!
    public virtual Player Player { get; set; } = null!;
}