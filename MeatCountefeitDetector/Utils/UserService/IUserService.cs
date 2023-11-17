using DataAccess.Models;
using System;

namespace MeatCounterfeitDetector.Utils.UserService
{
    public interface IUserService
    {
        void SetUserByToken(string token);
        bool IsAdmin { get; }
        bool IsTechnologist { get; }
         Guid CurrentUserId { get; }
    }
}