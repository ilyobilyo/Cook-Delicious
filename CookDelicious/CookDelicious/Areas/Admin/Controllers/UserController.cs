using AutoMapper;
using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.Models.Admin;
using CookDelicious.Core.Models.Admin.Comments;
using CookDelicious.Core.Models.Paiging;
using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
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
        private readonly IUserServiceAdmin userService;
        private readonly ICommentServiceAdmin commentService;
        private readonly IMapper mapper;

        public UserController(RoleManager<IdentityRole> roleManager,
            IUserServiceAdmin userService,
            UserManager<ApplicationUser> userManager,
            ICommentServiceAdmin commentService,
            IMapper mapper)
        {
            this.roleManager = roleManager;
            this.userService = userService;
            this.userManager = userManager;
            this.commentService = commentService;
            this.mapper = mapper;
        }

        [Authorize(Roles = UserConstraints.Roles.Administrator)]
        public async Task<IActionResult> ManageUsers(int pageNumber)
        {
            if (pageNumber == 0)
            {
                pageNumber = 1;
            }

            int pageSize = 2;

            (var usersServiceModels, var totalCount) = await userService.GetUsersPageingInManageUsers(pageNumber, pageSize);

            var usersViewModel = mapper.Map<List<UserListViewModel>>(usersServiceModels);

            var pageingList = new PagingList<UserListViewModel>(usersViewModel, totalCount, pageNumber, pageSize);

            return View(pageingList);
        }

        [Authorize(Roles = UserConstraints.Roles.Administrator)]
        public async Task<IActionResult> Roles(string id)
        {
            var user = await userService.GetUserByIdRoles(id);

            ViewBag.RoleItems = roleManager.Roles
                .ToList()
                .Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Name,
                    Selected = userManager.IsInRoleAsync(user, x.Name).Result
                })
                .ToList();

            var model = mapper.Map<UserRolesViewModel>(user);

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
            var serviceModel = await userService.GetUserByIdEdit(id);

            var viewModel = mapper.Map<UserEditViewModel>(serviceModel);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var serviceModel = mapper.Map<UpdateUserInputModel>(model);

            if (await userService.UpdateUser(serviceModel))
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

        [Authorize(Roles = UserConstraints.Roles.Administrator)]
        public async Task<IActionResult> RecipeComments([FromRoute] string id)
        {
            var commentsServiceModels = await commentService.GetRecipeComments(id);

            var commentsViewModel = mapper.Map<List<AdminCommentViewModel>>(commentsServiceModels);

            return View(commentsViewModel);
        }

        [Authorize(Roles = UserConstraints.Roles.Administrator)]
        public async Task<IActionResult> ForumComments([FromRoute] string id)
        {
            var commentsServiceModels = await commentService.GetForumComments(id);

            var commentsViewModel = mapper.Map<List<AdminCommentViewModel>>(commentsServiceModels);

            return View(commentsViewModel);
        }
    }
}
