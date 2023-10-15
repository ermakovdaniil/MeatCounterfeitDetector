using ClientAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientAPI.DTO.Login;

namespace MeatCountefeitDetector.Utils.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly AuthClient _authClient;

        private LoginResponse _LoginResponse;

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

        public async Task<string> Login(LoginModel model)
        {
            _authClient.LoginAsync(model);
            return null;
        }

        public string GetToken()
        {
            try
            {
                if (_LoginResponse is not null && _LoginResponse.Expiration <= DateTime.Now)
                {
                    _LoginResponse = _authClient.RefreshTokenAsync(new RefreshTokenModel { RefreshToken = _LoginResponse.RefreshToken, AccessToken = _LoginResponse.Token }).GetAwaiter().GetResult();
                }

                return _LoginResponse.Token;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
