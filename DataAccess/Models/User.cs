using DataAccess.Interfaces;
using System.Collections.ObjectModel;

using Microsoft.AspNetCore.Identity;

namespace DataAccess.Models
{
    public partial class User : IdentityUser<Guid>, IBaseEntity
    {
        public User()
        {
            Results = new ObservableCollection<Result>();
        }

        public Guid Id { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Name { get; set; } = null!;
        public Guid TypeId { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public virtual ObservableCollection<Result> Results { get; set; }
    }
}
