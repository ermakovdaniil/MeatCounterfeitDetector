using System.Collections.ObjectModel;


namespace DataAccess.Models;

public class ResultPath
{
    public ResultPath()
    {
        Results = new ObservableCollection<Result>();
    }

    public long Id { get; set; }
    public long? InitId { get; set; }
    public string Path { get; set; } = null!;

    public virtual OriginalPath? Init { get; set; }
    public virtual ObservableCollection<Result> Results { get; set; }
}
