using System.Collections.ObjectModel;


namespace DataAccess.Models;

public partial class User
{
    public User()
    {
        Results = new ObservableCollection<Result>();
    }

    public long Id { get; set; }
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Name { get; set; } = null!;
    public long TypeId { get; set; }

    public virtual UserType Type { get; set; } = null!;
    public virtual ObservableCollection<Result> Results { get; set; }
}
