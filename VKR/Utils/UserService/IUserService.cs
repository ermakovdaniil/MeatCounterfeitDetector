using DataAccess.Models;

namespace VKR.Utils.UserService
{
    public interface IUserService
    {
        User User { get; set; }

        bool IsAdmin { get; }
    }
}