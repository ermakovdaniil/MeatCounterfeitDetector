﻿using ClientAPI.DTO.Login;
using System.Threading.Tasks;

namespace MeatCountefeitDetector.Utils.AuthService
{
    public interface IAuthService
    {
        public Task<string> Login(LoginModel model);

        public string GetToken();
    }
}