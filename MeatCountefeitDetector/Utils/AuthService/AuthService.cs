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

        private RefreshTokenResponse _refreshTokenResponse;

        public AuthService(AuthClient authClient)
        {
            _authClient = authClient;
        }

        //public async Task<LoginResponse> GetToken()
        //{
        //    if (_refreshTokenResponse is not null && _refreshTokenResponse.Expiration <= DateTime.Now)
        //    {
        //        return _refreshTokenResponse;
        //    }

        //    if (_refreshTokenResponse is not null)
        //    {
        //        //_refreshTokenResponse = await _authClient.RefreshTokenAsync(new RefreshTokenModel{RefreshToken =_refreshTokenResponse.RefreshToken, AccessToken = _refreshTokenResponse.Token});
        //    }
        //    return _refreshTokenResponse;
        //}

        public async Task<string> AuthorizationWithLoginAndPassword(LoginModel model)
        {



            return null;
        }

        public string GetToken()
        {
            try
            {
                if (_refreshTokenResponse is not null && _refreshTokenResponse.Expiration <= DateTime.Now)
                {
                    _refreshTokenResponse = _authClient.RefreshTokenAsync(new RefreshTokenModel { RefreshToken = _refreshTokenResponse.RefreshToken, AccessToken = _refreshTokenResponse.Token }).GetAwaiter().GetResult();
                }

                return _refreshTokenResponse.Token;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
