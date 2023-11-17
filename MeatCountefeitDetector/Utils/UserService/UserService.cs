using DataAccess.Models;
using Newtonsoft.Json;
using System;
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

                return userJwtToken.Claims.Any(c => c.Type == $"http://schemas.microsoft.com/ws/2008/06/identity/claims/role" && c.Value.ToLower() == UserRolesConstants.Admin.ToLower());
            }
        }

        public bool IsTechnologist
        {
            get
            {
                if (userJwtToken is null)
                {
                    return false;
                }

                return userJwtToken.Claims.Any(c => c.Type == $"http://schemas.microsoft.com/ws/2008/06/identity/claims/role" && c.Value.ToLower() == UserRolesConstants.Technologist.ToLower());
            }
        }

        public Guid CurrentUserId
        {
            get
            {
                if (userJwtToken is null)
                {
                    return Guid.Parse("00000000-0000-0000-0000-000000000000");
                }

                var userIdClaim = userJwtToken.Claims.FirstOrDefault(claim => claim.Type == $"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

                Guid.TryParse(userIdClaim.Value, out Guid userId);

                return userId;
            }
        }

        private static string GetRoleClaimByName (string claimName)
        {
            return $"http://schemas.microsoft.com/ws/2008/06/identity/claims/{claimName}";           
        }

    }
}
