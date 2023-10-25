using DataAccess.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace MeatCounterfeitDetector.Utils.UserService
{
    public class UserService : IUserService
    {
        private JwtSecurityToken userJwtToken { get; set; }

        public void SetUserByToken(string token)
        {
            userJwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        }

        public bool IsAdmin
        {
            get
            {
                if (userJwtToken is null)
                {
                    return false;
                }

                return userJwtToken.Claims.Any(c => c.Type == $"http://schemas.microsoft.com/ws/2008/06/identity/claims/role" && c.Value.ToLower() == UserRoles.Admin.ToLower());
            }
        }

        private static string GetRoleClaimByName (string claimName)
        {
            return $"http://schemas.microsoft.com/ws/2008/06/identity/claims/{claimName}";           
        }

    }
}
