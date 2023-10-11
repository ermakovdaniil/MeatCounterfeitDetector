using DataAccess.Models;

namespace MeatCountefeitDetector.Utils.UserService
{
    public interface IUserService
    {
        User User { get; set; }

        //bool IsAdmin { get; }
    }
}