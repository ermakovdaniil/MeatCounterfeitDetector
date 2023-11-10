using DataAccess.Interfaces;
using System.Collections.ObjectModel;

using Microsoft.AspNetCore.Identity;

namespace DataAccess.Models
{
    public partial class User : IdentityUser<Guid>, IBaseEntity
    {
        Guid IBaseEntity.Id => this.Id;
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public virtual List<Result>? Results { get; set; }
        public virtual List<IdentityRole> IdentityRoles { get; set; }
    }
}
