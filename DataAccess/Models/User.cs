using DataAccess.Interfaces;
using System.Collections.ObjectModel;

namespace DataAccess.Models
{
    public partial class User : IBaseEntity
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

        public virtual UserType Type { get; set; } = null!;
        public virtual ObservableCollection<Result> Results { get; set; }
    }
}
