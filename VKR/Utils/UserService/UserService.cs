using DataAccess.Models;

namespace VKR.Utils.UserService
{
    public class UserService : IUserService
    {
        public User User { get; set; }

        public bool IsAdmin { get => User.Type.Name == "Администратор"; }
    }
}
