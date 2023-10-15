using DataAccess.Interfaces;
using System.Collections.ObjectModel;

using Microsoft.AspNetCore.Identity;

namespace DataAccess.Models
{
    public partial class User : IdentityUser<Guid>, IBaseEntity
    {
        Guid IBaseEntity.Id => this.Id;
        public string Name { get; set; } = null!;
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public virtual ObservableCollection<Result> Results { get; set; }
    }
}
