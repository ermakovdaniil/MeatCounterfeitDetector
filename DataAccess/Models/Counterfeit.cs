using DataAccess.Interfaces;
using System.Collections.ObjectModel;

namespace DataAccess.Models;

public class Counterfeit : IBaseEntity
{
    public Counterfeit()
    {
        CounterfeitPaths = new ObservableCollection<CounterfeitPath>();
    }

    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    public virtual ObservableCollection<CounterfeitPath> CounterfeitPaths { get; set; }
}

