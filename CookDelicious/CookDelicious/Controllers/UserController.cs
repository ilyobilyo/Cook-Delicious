using AutoMapper;
using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.User;
using CookDelicious.Core.Models.User;
using CookDelicious.Core.Service.Models.InputServiceModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Controllers
{
    [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.User}")]
    public class UserController : BaseController
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> MyProfile()
        {
            var serviceModel = await userService.GetUserByUsername(User.Identity.Name);

            var model = mapper.Map<UserProfileViewModel>(serviceModel);

            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> UserProfile([FromQuery]string AuthorName)
        {
            var serviceModel = await userService.GetUserByUsername(AuthorName);

            var model = mapper.Map<UserProfileViewModel>(serviceModel);

            return View(model);
        }

        public async Task<IActionResult> EditProfile()
        {
            var serviceModel = await userService.GetUserByUsername(User.Identity.Name);

            var model = mapper.Map<UserEditProfileViewModel>(serviceModel);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(UserEditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var inputModel = mapper.Map<UserEditProfileInputModel>(model);

            if (await userService.UpdateUser(inputModel))
            {
                ViewData[MessageConstant.SuccessMessage] = MessageConstant.SuccessfulRecord;
            }
            else
            {
                ViewData[MessageConstant.ErrorMessage] = MessageConstant.OccurredError;
            }

            return View(model);
        }
    }
}
