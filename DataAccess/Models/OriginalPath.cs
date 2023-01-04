using System.Collections.ObjectModel;

namespace DataAccess.Models;

public partial class OriginalPath
{
    public OriginalPath()
    {
        ResultPaths = new ObservableCollection<ResultPath>();
        Results = new ObservableCollection<Result>();
    }

    public long Id { get; set; }
    public string Path { get; set; } = null!;

    public virtual ObservableCollection<ResultPath> ResultPaths { get; set; }
    public virtual ObservableCollection<Result> Results { get; set; }
}