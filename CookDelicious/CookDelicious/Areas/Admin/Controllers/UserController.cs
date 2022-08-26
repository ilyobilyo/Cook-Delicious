using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.Models.Admin;
using CookDelicious.Core.Models.Paiging;
using CookDelicious.Infrasturcture.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CookDelicious.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly RoleManager<IdentityRole> roleManager;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly IUserService userService;

        public UserController(RoleManager<IdentityRole> roleManager, IUserService userService, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userService = userService;
            this.userManager = userManager;
        }

        [Authorize(Roles = UserConstraints.Roles.Administrator)]
        public async Task<IActionResult> ManageUsers(int pageNumber)
        {
            var users = await userService.GetUsersInManageUsers(pageNumber);

            return View(users);
        }

        [Authorize(Roles = UserConstraints.Roles.Administrator)]
        public async Task<IActionResult> Roles(string id)
        {
            var user = await userService.GetUserByIdRoles(id);

            var model = new UserRolesViewModel()
            {
                Id = id,
                Username = user.UserName
            };

            ViewBag.RoleItems = roleManager.Roles
                .ToList()
                .Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Name,
                    Selected = userManager.IsInRoleAsync(user, x.Name).Result
                })
                .ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Roles(UserRolesViewModel model)
        {
            var user = await userService.GetUserByIdRoles(model.Id);

            var userRoles = await userManager.GetRolesAsync(user);

            await userManager.RemoveFromRolesAsync(user, userRoles);

            if (model.RoleNames?.Length > 0)
            {
                await userManager.AddToRolesAsync(user, model.RoleNames);
            }

            return Redirect("/Admin/User/ManageUsers");
        }

        [Authorize(Roles = UserConstraints.Roles.Administrator)]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await userService.GetUserByIdEdit(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel model)
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

        [Authorize(Roles = UserConstraints.Roles.Administrator)]
        public async Task<IActionResult> CreateRole()
        {
            await roleManager.CreateAsync(new IdentityRole()
            {
                Name = "User"
            });

            return Redirect("/");
        }
    }
}
