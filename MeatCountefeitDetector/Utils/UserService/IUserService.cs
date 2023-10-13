using DataAccess.Models;

namespace MeatCounterfeitDetector.Utils.UserService
{
    public interface IUserService
    {
        User User { get; set; }

        //bool IsAdmin { get; }
    }
}