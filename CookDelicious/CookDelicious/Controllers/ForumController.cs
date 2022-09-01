using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Forum;
using CookDelicious.Core.Contracts.User;
using CookDelicious.Core.Models.Forum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Controllers
{
    public class ForumController : BaseController
    {
        private readonly IForumService forumService;

        public ForumController(IForumService forumService)
        {
            this.forumService = forumService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Home()
        {
            var posts = await forumService.GetAllPosts();

            var categories = await forumService.GetAllPostCategoryNames();

            var archive = await forumService.GetArchive();

            var model = new ForumHomeViewModel()
            {
                Categories = categories,
                Posts = posts,
                Archive = archive
            };

            return View(model);
        }

        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> CreatePost()
        {
            var categories = await forumService.GetAllPostCategoryNames();

            var model = new CreatePostViewModel()
            {
                Categories = categories,
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> CreatePost(CreatePostViewModel model)
        {
            var error = await forumService.CreatePost(model, User.Identity.Name);

            if (error != null)
            {
                ViewData[MessageConstant.ErrorMessage] = error.Messages;
            }
            else
            {
                ViewData[MessageConstant.SuccessMessage] = "Вие публикувахте поста успешно!";
            }

            var categories = await forumService.GetAllPostCategoryNames();

            model.Categories = categories;

            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ForumPost([FromRoute]Guid Id)
        {
            var post = await forumService.GetPostById(Id);

            return View(post);
        }
    }
}
