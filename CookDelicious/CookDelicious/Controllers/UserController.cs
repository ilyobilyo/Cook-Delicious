using CookDelicious.Core.Contracts;
using CookDelicious.Core.Contracts.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Controllers
{
    [Authorize(Roles = "User, Administrator")]
    public class UserController : BaseController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<IActionResult> Profile()
        {
            //var model = userService.GetUserProfile(User.Identity.Name);

            return View();
        }
    }
}
