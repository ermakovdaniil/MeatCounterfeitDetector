using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class UserType
    {
        public UserType()
        {
            Users = new HashSet<User>();
        }

        public long Id { get; set; }
        public string Type { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
