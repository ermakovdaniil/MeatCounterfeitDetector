namespace DataAccess.Models;

public class User
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Password { get; set; } = null!;
    public long TypeId { get; set; }

    public virtual UserType Type { get; set; } = null!;
}
