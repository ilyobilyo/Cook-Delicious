using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly RoleManager<IdentityRole> roleManager;

        private readonly IUserService userService;

        public UserController(RoleManager<IdentityRole> roleManager, IUserService userService)
        {
            this.roleManager = roleManager;
            this.userService = userService;
        }

        [Authorize(Roles = UserConstraints.Roles.Administrator)]
        public async Task<IActionResult> ManageUsers()
        {
            var users = await userService.GetUsers();

            return View(users);
        }


        [Authorize(Roles = UserConstraints.Roles.Administrator)]
        public async Task<IActionResult> CreateRole()
        {
            await roleManager.CreateAsync(new IdentityRole()
            {
                Name = "Administrator"
            });

            return Redirect("/");
        }
    }
}
