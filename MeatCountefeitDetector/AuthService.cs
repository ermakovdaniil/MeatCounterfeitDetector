using ClientAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientAPI.DTO.Login;

namespace MeatCountefeitDetector
{
    public class AuthService
    {
        private readonly AuthClient _authClient;

        private LoginResponse _loginResponse;

        public AuthService(AuthClient authClient)
        {
            _authClient = authClient;
        }

        public async Task<LoginResponse> GetToken()
        {
            if (_loginResponse is not null && _loginResponse.Expiration <= DateTime.Now)
            {
                return _loginResponse;
            }

            if (_loginResponse is not null)
            {
                //_loginResponse = await _authClient.RefreshTokenAsync(new RefreshTokenModel{RefreshToken =_loginResponse.RefreshToken, AccessToken = _loginResponse.Token});
            }
            return _loginResponse;
        }
    }
}
