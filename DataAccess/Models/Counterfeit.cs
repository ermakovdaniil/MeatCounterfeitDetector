using DataAccess.Interfaces;
using System.Collections.ObjectModel;

namespace DataAccess.Models;

public class Counterfeit : IBaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    public virtual List<CounterfeitImage> CounterfeitImages { get; set; }
}