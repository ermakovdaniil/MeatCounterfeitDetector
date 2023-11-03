using DataAccess.Interfaces;

namespace DataAccess.Models;

public class CounterfeitPath : IBaseEntity
{
    public Guid Id { get; set; }
    public Guid CounterfeitId { get; set; }
    public string ImagePath { get; set; } = null!;

    public virtual Counterfeit Counterfeit { get; set; } = null!;
}