using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts;
using CookDelicious.Core.Contracts.User;
using CookDelicious.Core.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Controllers
{
    [Authorize(Roles = "Administrator, User")]
    public class UserController : BaseController
    {
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<IActionResult> Profile()
        {
            var model = await userService.GetUserProfile(User.Identity.Name);

            return View(model);
        }

        public async Task<IActionResult> EditProfile()
        {
            var model = await userService.GetUserProfileEdit(User.Identity.Name);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(UserEditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await userService.UpdateUser(model))
            {
                ViewData[MessageConstant.SuccessMessage] = "Успешен запис!";
            }
            else
            {
                ViewData[MessageConstant.ErrorMessage] = "Възникна грешка!";
            }

            return View(model);
        }
    }
}
