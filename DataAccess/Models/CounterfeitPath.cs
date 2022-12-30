namespace DataAccess.Models;

public class CounterfeitPath
{
    public long Id { get; set; }
    public long CounterfeitId { get; set; }
    public string ImagePath { get; set; } = null!;

    public virtual Counterfeit Counterfeit { get; set; } = null!;
}
