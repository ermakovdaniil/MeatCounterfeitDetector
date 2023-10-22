using ClientAPI.DTO.Login;
using System.Threading.Tasks;

namespace MeatCounterfeitDetector.Utils.AuthService
{
    public interface IAuthService
    {
        public Task LoginAsync(LoginModel model);

        public string GetToken();
    }
}