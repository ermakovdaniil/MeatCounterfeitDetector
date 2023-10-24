using ClientAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientAPI.DTO.Login;

namespace MeatCounterfeitDetector.Utils.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly AuthClient _authClient;

        private LoginResponse _loginResponse;

        public AuthService(AuthClient authClient)
        {
            _authClient = authClient;
        }

        //public async Task<LoginResponse> GetToken()
        //{
        //    if (_LoginResponse is not null && _LoginResponse.Expiration <= DateTime.Now)
        //    {
        //        return _LoginResponse;
        //    }

        //    if (_LoginResponse is not null)
        //    {
        //        //_LoginResponse = await _authClient.RefreshTokenAsync(new RefreshTokenModel{RefreshToken =_LoginResponse.RefreshToken, AccessToken = _LoginResponse.Token});
        //    }
        //    return _LoginResponse;
        //}

        public async Task LoginAsync(LoginModel model)
        {
            _loginResponse = await _authClient.LoginAsync(model);
        }

        public string GetToken()
        {
            if (_loginResponse is not null && _loginResponse.Expiration <= DateTime.Now)
            {
                _loginResponse = _authClient.RefreshTokenAsync(new RefreshTokenModel { RefreshToken = _loginResponse.RefreshToken, AccessToken = _loginResponse.Token }).Result;
            }
            if (_loginResponse is not null)
            {
                return _loginResponse.Token;
            }
            return null;
        }
    }
}
