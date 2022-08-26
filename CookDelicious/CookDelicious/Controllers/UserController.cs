using CookDelicious.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Controllers
{
    [Authorize(Roles = "User, Administrator")]
    public class UserController : BaseController
    {

    }
}
