using DataAccess;
using DataAccess.Models;
using MeatAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace WebAppWithReact.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseAuthorizedController : ControllerBase
{
    public bool IsUserAdmin => User.IsInRole(UserRoles.Admin);
    protected Guid UserId => User.GetLoggedInUserId<Guid>();
}
