using System.Collections.ObjectModel;

namespace DataAccess.Models;

public partial class Result
{
    public long Id { get; set; }
    public string Date { get; set; } = null!;
    public long UserId { get; set; }
    public string AnRes { get; set; } = null!;
    public long OrigPathId { get; set; }
    public long ResPathId { get; set; }

    public virtual OriginalPath OrigPath { get; set; } = null!;
    public virtual ResultPath ResPath { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}

