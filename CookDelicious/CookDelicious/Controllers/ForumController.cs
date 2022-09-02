using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Comments;
using CookDelicious.Core.Contracts.Forum;
using CookDelicious.Core.Contracts.User;
using CookDelicious.Core.Models.Comments;
using CookDelicious.Core.Models.Forum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Controllers
{
    public class ForumController : BaseController
    {
        private readonly IForumService forumService;
        private readonly ICommentService commentService;

        public ForumController(IForumService forumService, ICommentService commentService)
        {
            this.forumService = forumService;
            this.commentService = commentService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Home(int pageNumber)
        {
            var posts = await forumService.GetAllPosts(pageNumber);

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
        public async Task<IActionResult> ForumPost([FromRoute]Guid Id, int commentPage)
        {
            var post = await forumService.GetPostById(Id, commentPage);

            return View(post);
        }

        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> DeletePost([FromRoute]Guid Id)
        {
            var IsDeleted = await forumService.DeletePost(Id);

            if (!IsDeleted)
            {
                return BadRequest("Неуспешно изтриване!");
            }

            return RedirectToAction(nameof(Home));
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> PostComment([FromRoute] Guid Id, CommentViewModel model)
        {
            var comment = await commentService.PostCommentForPost(Id, model);

            return RedirectToAction(nameof(ForumPost), new { Id = Id });
        }

        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> DeletePostComment([FromRoute] Guid Id, [FromQuery] Guid postId)
        {
            var IsDeleted = await commentService.DeletePostComment(Id);

            if (!IsDeleted)
            {
                return BadRequest("Неуспешно изтриване!");
            }

            return RedirectToAction(nameof(ForumPost), new { Id = postId });
        }
    }
}
