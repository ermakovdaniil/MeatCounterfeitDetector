using System.Collections.ObjectModel;

namespace DataAccess.Models;

public class Counterfeit
{
    public Counterfeit()
    {
        CounterfeitPaths = new ObservableCollection<CounterfeitPath>();
    }

    public long Id { get; set; }
    public string Name { get; set; } = null!;

    public virtual ObservableCollection<CounterfeitPath> CounterfeitPaths { get; set; }
}

