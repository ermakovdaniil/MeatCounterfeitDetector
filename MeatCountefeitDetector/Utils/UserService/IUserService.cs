using DataAccess.Models;

namespace MeatCounterfeitDetector.Utils.UserService
{
    public interface IUserService
    {
        void SetUserByToken(string token);
        bool IsAdmin { get; }
    }
}