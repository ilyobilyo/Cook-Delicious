using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.Models.Admin.Forum;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Areas.Admin.Controllers
{
    public class ForumController : BaseController
    {
        private readonly IForumServiceAdmin forumService;

        public ForumController(IForumServiceAdmin forumService)
        {
            this.forumService = forumService;
        }

        public IActionResult CreatePostCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePostCategory(CreatePostCategoryViewModel model)
        {
            var error = await forumService.CreatePostCategory(model);

            if (error != null)
            {
                ViewData[MessageConstant.ErrorMessage] = error.Messages;
                return View(model);
            }
            else
            {
                ViewData[MessageConstant.SuccessMessage] = "Успешен запис";
            }

            return View();
        }
    }
}
